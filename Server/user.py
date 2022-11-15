import socket

class User(object):

    def __init__(self, i: str, u: str, s: socket):
        self._id = i
        self._username = u
        self._socket = s

    def addSocketInfo(self, s:socket):
        self._socket = s

    def __str__(self):
        return "{}/{}".format(self._id, self._username)
