using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    const string JOIN = "/";

    public string id;
    public string username;

    public User(string u)
    {
        string[] userParams = u.Split(new string[] { JOIN }, StringSplitOptions.None);
        this.id = userParams[0];
        this.username = userParams[1];
    }
}
