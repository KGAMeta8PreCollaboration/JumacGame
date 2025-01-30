using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JegiUserData
{
    public string nickname;
    public int score;

    public JegiUserData() { }

    public JegiUserData(string nickName, int score)
    {
        this.nickname = nickName;
        this.score = score;
    }
}
