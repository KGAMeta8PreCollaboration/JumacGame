using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JegiUserData
{
    public string nickName;
    public int score;

    public JegiUserData() { }

    public JegiUserData(string nickName, int score)
    {
        this.nickName = nickName;
        this.score = score;
    }
}
