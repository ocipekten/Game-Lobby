import socket
from message import Message

class Protocol(object):
    BUFSIZE = 8196

    def __init__(self, s: socket=-1):
        self._sock = s

    def _loopRecv(self, size: int):
        data = bytearray(b" "*size)
        mv = memoryview(data)
        while size:
            rsize = self._sock.recv_into(mv,size)
            mv = mv[rsize:]
            size -= rsize
        return data

    def putMessage(self, m: Message):
        data = m.marshal()
        self._sock.sendall(data.encode('utf-8'))

    def getMessage(self) -> Message:
        try:
            m = Message()
            size = int(self._loopRecv(4).decode('utf-8'))
            mtype = self._loopRecv(4).decode('utf-8')
            params = self._loopRecv(size).decode('utf-8')
            m.unmarshal(params)
            m.setType(mtype)
        except Exception:
            raise Exception('bad getMessage')
        else:
            return m

    def close(self):
        self._sock.close()
