using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogInUserData
{
    public string id;
    public string timestamp;
    public string nickname;
    public string race;

    public LogInUserData(string id, string timestamp = "", string nickname = "", string race = "")
    {
        this.id = id;
        this.timestamp = timestamp;
        this.nickname = nickname;
        this.race = race;
    }

    public LogInUserData() { }
}
