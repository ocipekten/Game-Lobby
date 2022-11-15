from user import User

class Lobby(object):

      MAX_PLAYERS = 4

      MJOIN = ','

      def __init__(self, i: str, n: str):
            self._id = i
            self._name = n
            self._members = {}
            self._started = False

      def setOwner(self, o: User):
            self._owner = o

      def addMember(self, m: User):
            self._members[m._id] = m

      def removeMember(self, m: User) -> bool:
            if self._owner == m and len(list(self._members.keys())) > 0:
                  self._owner = list(self._members.values())[0]
                  self._members.pop(self._owner._id)
                  return True
            elif self._owner == m:
                  return False
            else:
                  self._members.pop(m._id)
                  return True

      def start(self):
            self._started = True

      def isOwner(self, m:User) -> int:
            if self._owner == m:
                  return 1
            else:
                  return 0

      def membersStr(self):
            if len(self._members) == 0:
                  return ""
            else:
                  params = Lobby.MJOIN.join(str(v) for v in self._members.values())
                  return params
      
      def __str__(self):
            return "{}-{}-{}".format(self._id, self._name, self._owner)