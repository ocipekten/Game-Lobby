using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClientOperations
{
    private Protocol protocol;
    private string host;
    private int port;

    private bool login;
    private bool done;

    private User user;

    public ClientOperations()
    {
        protocol = null;
        login = false;
        user = null;
    }

    private void SetProtocol(Protocol p)
    {
        protocol = p;
    }

    public void SetServer(string h, int p)
    {
        host = h;
        port = p;
    }

    public void Connect()
    {
        Socket comm = new Socket(SocketType.Stream, ProtocolType.Tcp);
        try
        {
            comm.Connect(host, port);
            SetProtocol(new Protocol(comm));
        }
        catch (Exception e)
        {
            Spawner.SpawnError(e.Message);
            return;
        }        
    }

    public void Disconnect()
    {
        protocol.Close();
    }

    public bool DoLogin(string uname, string password)
    {
        Message req = new Message();
        req.SetType(MCmds.LGIN);
        req.AddParameter("username", uname);
        req.AddParameter("password", password);

        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            if (resp.GetType().CompareTo(MCmds.GOOD.ToString()) == 0)
            {
                this.user = new User(resp.GetParameter("user"));
                return true;
            }
            else
            {
                Spawner.SpawnError(resp.GetParameter("message"));
                Debug.LogError(resp.GetParameter("message"));
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return false;
    }

    public void DoLogout()
    {
        Message req = new Message();
        req.SetType(MCmds.LOUT);
        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            login = false;
            protocol.Close();
            this.user = null;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void DoSignup(string u, string p)
    {
        Message req = new Message();
        req.SetType(MCmds.SIGN);
        req.AddParameter("username", u);
        req.AddParameter("password", p);
        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            if (resp.GetType().CompareTo(MCmds.GOOD.ToString()) == 0)
            {
                Debug.Log(resp.GetParameter("message"));
            }
            else
            {
                Spawner.SpawnError(resp.GetParameter("message"));
                Debug.Log(resp.GetParameter("message"));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public Lobby DoCreatelobby(string n)
    {
        Message req = new Message();
        req.SetType(MCmds.CREL);
        req.AddParameter("name", n);

        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            if (resp.GetType().CompareTo(MCmds.DATA.ToString()) == 0)
            {
                return new Lobby(resp.GetParameter("lobby"));
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return null;
    }

    public Dictionary<int, Lobby> DoGetLobbies()
    {
        Message req = new Message();
        req.SetType(MCmds.GEAL);

        Dictionary<int, Lobby> allLobbies = new Dictionary<int, Lobby>();

        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            int length = Int32.Parse(resp.GetParameter("length"));
            for (int i = 0; i < length; i++)
            {
                Lobby newlobby = new Lobby(resp.GetParameter(i.ToString()));
                allLobbies[Int32.Parse(newlobby.id)] = newlobby;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return allLobbies;
    }

    public Lobby DoJoinLobby(Lobby l)
    {
        Message req = new Message();
        req.SetType(MCmds.JOIL);
        req.AddParameter("id", l.id);

        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            return new Lobby(resp.GetParameter("lobby"));
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return null;
    }

    public (Lobby,bool) DoGetLobby()
    {
        Message req = new Message();
        req.SetType(MCmds.GETL);
        
        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            Lobby lobby = new Lobby(resp.GetParameter("lobby"));
            lobby.AddPlayers(resp.GetParameter("members"));
            bool isOwner = Convert.ToBoolean(Int32.Parse(resp.GetParameter("isOwner")));
            return (lobby,isOwner);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return (null,false);
    }

    public void DoExitLobby()
    {
        Message req = new Message();
        req.SetType(MCmds.EXIL);

        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void DoStartLobby()
    {
        Message req = new Message();
        req.SetType(MCmds.STAL);

        try
        {
            protocol.PutMessage(req);
            Message resp = protocol.GetMessage();
            if (resp.GetType().CompareTo(MCmds.GOOD.ToString()) == 0)
            {
                Debug.Log("GAME STARTED!");
            }
            else
            {
                Debug.LogError(resp.GetParameter("message"));
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}