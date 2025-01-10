using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OmokUserData : MonoBehaviour
{
    public string id;
    public string nickname;
    public float gold;

    public OmokUserData() { }

    public OmokUserData(string id, string nickname, float gold)
    {
        this.id = id;
        this.nickname = nickname;
        this.gold = gold;
    }
}
