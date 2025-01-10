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
    public string serverName;
    public string timestamp;

    public ChatUserData()
    {
        id = string.Empty;
        nickname = "";
        serverName = "";
        timestamp = DateTime.UtcNow.ToString("o");
    }
    public ChatUserData(string id, string nickname, string serverName, string timestamp)
    {
        this.id = id;
        this.nickname = nickname;
        this.serverName = serverName;
        this.timestamp = timestamp;
    }
}