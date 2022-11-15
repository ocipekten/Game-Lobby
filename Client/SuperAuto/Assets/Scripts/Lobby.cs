using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby
{
    const int MAX_PLAYERS = 4;
    const string JOIN = "-";
    const string MJOIN = ",";

    public string id;
    public string name;
    public User owner;
    private List<User> members;

    public Lobby(string m)
    {
        string[] lobbyParams = m.Split(new string[] { JOIN }, StringSplitOptions.None);
        this.id = lobbyParams[0];
        this.name = lobbyParams[1];

        User owner = new User(lobbyParams[2]);
        this.owner = owner;

        this.members = new List<User>();
    }

    public void AddPlayers(string m)
    {
        this.members.Clear();
        if (m == "") return;
        string[] membersParams = m.Split(new string[] { MJOIN }, StringSplitOptions.None);
        foreach (string param in membersParams)
        {
            members.Add(new User(param));
        }
    }

    public List<User> GetPlayers()
    {
        return members;
    }
}
