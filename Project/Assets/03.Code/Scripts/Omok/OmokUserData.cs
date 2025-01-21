using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OmokUserData
{
    public string id;
    public string nickname;
    public float gold;
    public int win;
    public int lose;

    public OmokUserData() { }

    public OmokUserData(string id)
    {
        this.id = id;
        nickname = "";
        gold = 0;
        win = 0;
        lose = 0;
    }

    public OmokUserData(string id, string nickname, float gold)
    {
        this.id = id;
        this.nickname = nickname;
        this.gold = gold;
        this.win = 0;
        this.lose = 0;
    }
}
