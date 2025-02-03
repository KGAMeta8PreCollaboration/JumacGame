using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class ChatUserData
{
    public string id;
    public string nickname;
    public string servername;
    public string timestamp;

    public ChatUserData()
    {
        id = string.Empty;
        nickname = "";
        servername = "";
        timestamp = DateTime.UtcNow.ToString("o");
    }
    public ChatUserData(string id, string nickname, string serverName, string timestamp)
    {
        this.id = id;
        this.nickname = nickname;
        this.servername = serverName;
        this.timestamp = timestamp;
    }
}