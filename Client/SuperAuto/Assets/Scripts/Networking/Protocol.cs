using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System;
using System.Text;

public class Protocol
{
    private Socket socket;
    private NetworkStream ioStream;

    private bool debug;

    private void Setup(Socket s)
    {
        this.socket = s;
        this.ioStream = new NetworkStream(socket);
        this.debug = false;
    }

    public Protocol()
    {
        this.socket = null;
        this.ioStream = null;
    }

    public Protocol(Socket s)
    {
        Setup(s);
    }

    public void Close()
    {
        ioStream.Close();
        socket.Close();
    }

    public bool PutMessage(Message m)
    {
        bool ret = true;

        string mess = m.Marshal();
        if (ioStream != null) {
            ioStream.Write(Encoding.UTF8.GetBytes(mess), 0, Encoding.UTF8.GetBytes(mess).Length);
            ioStream.Flush();
        }

        if (this.debug) Debug.Log("Sent:" + m.ToString());

        return ret;
    }

    public Message GetMessage()
    {
        Message ret = null;
        if (ioStream != null)
        {
            byte[] size = new byte[4];
            ioStream.Read(size, 0, 4);
            byte[] type = new byte[4];
            ioStream.Read(type, 0, 4);
            int dsize = int.Parse(Encoding.UTF8.GetString(size));
            byte[] data = new byte[dsize];
            ioStream.Read(data, 0, dsize);
            ret = new Message(Encoding.UTF8.GetString(data));
            ret.SetType((MCmds)Enum.Parse(typeof(MCmds), Encoding.UTF8.GetString(type)));
        }

        if (this.debug) Debug.Log("Recv:" + ret.ToString());

        return ret;
    }
}
