import socket, mysql.connector

from message import Message
from protocol import Protocol
from user import User
from lobby import Lobby

class Serverops(object):
    
    GAME_SERVER = '172.31.3.164'
    GAME_PORT = 50002
    GAME_ADDR = (GAME_SERVER,GAME_PORT)


    lobbies = {}

    def __init__(self):
        self._user = None
        self._lobby = None
        
        self.sproto = Protocol()
        
        self.connected = False
        self._login = False
        self._route = {
            'LGIN': self._doLogin,
            'LOUT': self._doLogout,
            'SIGN': self._doSignup,
            'CREL': self._doCreatelobby,
            'GEAL': self._doGetLobbies,
            'GETL': self._doGetLobby,
            'JOIL': self._doJoinLobby,
            'EXIL': self._doExitLobby,
            'STAL': self._doStartLobby,
            'GELS': self._doGetLobbyStarted}
        self._debug = True

    def _debugPrint(self, m: str):
        if self._debug:
            print(m)

    def connect_db(self):
        try:
            self._db = mysql.connector.connect(
                user='admin',
                password='Ozanozan123*',
                host='superauto.cbtrbwwvqy0c.us-east-1.rds.amazonaws.com',
                database='Superauto',
                port=3306)
        except mysql.connector.Error as err:
            print(err)
            self.connected = False
        self._cursor = self._db.cursor(buffered=True)

    def connectToGameServer(self):
        gamesoc = socket.socket()
        gamesoc.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR,1)
        gamesoc.bind(Serverops.ADDR)
        gamesoc.listen(1)

    def _doLogin(self, req: Message) -> Message:
        u = req.getParam('username')
        p = req.getParam('password')

        resp = Message()

        findQuery = "SELECT * FROM Users WHERE username=%s AND password =%s"
        self._cursor.execute(findQuery,(u,p))
        findResult = self._cursor.fetchone()

        if findResult is None:
            resp.setType('ERRO')
            resp.addParam('message','User not found')
        else:
            if findResult[3] == 0:
                updateStatement = "UPDATE Users SET loggedIn=%s WHERE id=%s"
                self._cursor.execute(updateStatement, (1,findResult[0],))
                self._db.commit()

                self._user = User(findResult[0],findResult[1],self.sproto._sock)
                resp.setType('GOOD')
                resp.addParam('message','Login successful')
                resp.addParam('user', self._user)
                self._login = True
            else:
                resp.setType('ERRO')
                resp.addParam('message','User already logged in')
        return resp

    def _doLogout(self, req: Message) -> Message:
        resp = Message()

        updateStatement = "UPDATE Users SET loggedIn=%s WHERE id=%s"
        self._cursor.execute(updateStatement, (0,self._user._id,))
        self._db.commit()

        resp.setType('GOOD')
        resp.addParam('message','Logout successful')
        self._login = False
        self.connected = False
        return resp

    def _doSignup(self, req:Message) -> Message:
        u = req.getParam('username')
        p = req.getParam('password')

        resp = Message()

        insertStatement = "INSERT INTO Users(username,password) VALUES (%s,%s)"
        try:
            self._cursor.execute(insertStatement, (u,p))
            self._db.commit()
            resp.setType('GOOD')
            resp.addParam('message','Signup successful')
        except:
            resp.setType('ERRO')
            resp.addParam('message','Error signing up')
        return resp

    def _doCreatelobby(self, req:Message) -> Message:
        n = req.getParam('name')
        
        resp = Message()

        try:
            new_lobby = Lobby(len(Serverops.lobbies), n)
            self._lobby = new_lobby
            self._lobby.setOwner(self._user)
            Serverops.lobbies[self._lobby._id] = self._lobby
            resp.setType('DATA')
            resp.addParam('lobby', self._lobby)
        except Exception as e:
            print(e)
            resp.setType('ERRO')
            resp.addParam('message', 'Problem occurred')

        return resp

    def _doGetLobbies(self, req:Message) -> Message:
        resp = Message()

        try:
            for key, value in Serverops.lobbies.items():
                resp.addParam(key, value)

            resp.setType('DATA')
            resp.addParam('length', len(Serverops.lobbies))            
        except:
            resp.setType('ERRO')
            resp.addParam('message', 'Problem occurred')

        return resp

    def _doGetLobby(self, req:Message) -> Message:
        resp = Message()

        try:
            resp.setType('DATA')
            resp.addParam('lobby', self._lobby)
            resp.addParam('members', self._lobby.membersStr())
            resp.addParam('isOwner', str(self._lobby.isOwner(self._user)))
        except Exception as e:
            print(e)
            resp.setType('ERRO')
            resp.addParam('message', e)
        return resp

    def _doJoinLobby(self, req:Message) -> Message:
        id = req.getParam('id')
        id = int(id)
        resp = Message()

        try:
            self._lobby = Serverops.lobbies[id]
            self._lobby.addMember(self._user)
            resp.setType('DATA')
            resp.addParam('lobby', self._lobby)
        except Exception as e:
            print(e)
            resp.setType('ERRO')
            resp.addParam('message', 'Problem ocurred')
        
        return resp

    def _doExitLobby(self, req:Message) -> Message:
        resp = Message()

        if self._lobby == None:
            resp.setType('ERRO')
            resp.addParam('message','Not in lobby')
            return resp

        try:
            dontRemove = self._lobby.removeMember(self._user)
            if (dontRemove == False):
                Serverops.lobbies.pop(self._lobby._id)
            self._lobby = None
            resp.setType('GOOD')
            resp.addParam('message', 'Exited lobby')
        except Exception as e:
            print(e)
            resp.setType('ERRO')
            resp.addParam('message', 'Problem ocurred')
        
        return resp

    def _doStartLobby(self, req:Message) -> Message:
        resp = Message()

        try:
            Serverops.lobbies.pop(self._lobby._id)
            self._lobby.start()
            resp.setType('GOOD')
            resp.addParam('message', 'Started lobby')
        except Exception as e:
            resp.setType('ERRO')
            resp.addParam('message', e)

        return resp

    def _doGetLobbyStarted(self, req:Message) -> Message:
        resp = Message()
        try:
            resp.setType('GOOD')
            resp.addParam('started',str(self._lobby._started))
        except Exception as e:
            resp.setType('ERRO')
            resp.addParam('message', e)

        return resp

    def _process(self, req: Message) -> Message:
        m = self._route[req.getType()]
        return m(req)

    def shutdown(self):
        self.sproto.close()
        self._cursor.close()
        self._db.close()
        self._login = False
        self.connected = False

    def run(self):
        try:
            while (self.connected):
                # get message
                req = self.sproto.getMessage()
                self._debugPrint(req)
                # process request
                resp = self._process(req)
                self._debugPrint(resp)
                # send response
                self.sproto.putMessage(resp)
        except Exception as e:
            print(e)

        self.shutdown()