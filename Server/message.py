from enum import Enum

class Message(object):
    # Constance
    MCMDS = Enum('MCMDS', {'LGIN':'LGIN', 'LOUT': 'LOUT','SIGN':'SIGN', 
        'CREL':'CREL', 'GEAL':'GEAL', 'GETL':'GETL',
        'JOIL':'JOIL', 'EXIL':'EXIL', 'STAL':'STAL',
        'DATA': 'DATA', 'GOOD': 'GOOD', 'ERRO': 'ERRO','GELS':'GELS',
        })

    PJOIN = '&'
    VJOIN = '{}={}'
    VJOIN1 = '='

    def __init__(self):
        self._type = Message.MCMDS['GOOD']
        self._params = {}
    
    def __str__(self) -> str:
        return self.marshal()
    
    def reset(self):
        self._type = Message.MCMDS['GOOD']
        self._params.clear()
        self._params = {}
    
    def setType(self, mtype: str):
        self._type = Message.MCMDS[mtype]
        
    def getType(self) -> str:
        return self._type.value
    
    def addParam(self, name: str, value: str):
        self._params[name] = value
        
    def getParam(self, name: str) -> str:
        return self._params[name]
    
    def marshal(self) -> str:
        size = 0
        params = ''
        if (self._params):        
            pairs = [Message.VJOIN.format(k,v) for (k, v) in self._params.items()]
            params = Message.PJOIN.join(pairs)
            size = len(params)
        return '{:04}{}{}'.format(size,self._type.value,params)
    
    def unmarshal(self, value: str):
        self.reset()
        if value:
            params = value.split(Message.PJOIN)
            for p in params:
                k,v = p.split(Message.VJOIN1)
                self._params[k] = v
    
