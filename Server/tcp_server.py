import socket, threading, selectors
from serverops import Serverops
from protocol import Protocol

SERVER = '172.31.3.164'
PORT = 50001
ADDR = (SERVER,PORT)

GAME_SERVER = '172.31.3.164'
GAME_PORT = 50002
GAME_ADDR = (SERVER, PORT)

def thr_run(sops: Serverops):
    print("Started thread ", threading.current_thread())
    sops.run()
    print("Ended thread ", threading.current_thread())   

def start_server():
    serversoc = socket.socket()
    serversoc.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR,1)
    serversoc.bind(ADDR)
    serversoc.listen(6)
    thnum = 1

    while True:
        print('Listening on ', PORT)
        commsoc, raddr = serversoc.accept()

        sops = Serverops()
        sops.sproto = Protocol(commsoc)
        sops.connect_db()
        sops.connected = True

        tid = threading.Thread(name="thr_{}".format(thnum), target=thr_run, args=(sops,))
        print(tid.name)
        thnum = thnum + 1
        tid.setDaemon(True)
        tid.start()

    serversoc.close()

if __name__ == '__main__':
    start_server()
