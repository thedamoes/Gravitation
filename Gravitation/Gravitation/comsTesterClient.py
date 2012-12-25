# Echo server program
import socket

HOST = '127.0.0.1'                 # Symbolic name meaning all available interfaces
PORT = 50002              # Arbitrary non-privileged port

def sendString(s,string):
        s.send(string+"\r\n")
        return s.recv(1024)
    

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
s.connect((HOST, PORT))



print 'Connected '
inpt = ""
while inpt != "dc\n":
    inpt = raw_input('reply:')
    sendString(s,inpt)
s.close()
