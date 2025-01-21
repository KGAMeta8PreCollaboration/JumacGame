using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RGLightUserData
{
    public string nickname;
    public int score;

    public RGLightUserData(string nickname, int score = default)
    {
        this.nickname = nickname;
        this.score = score;
    }

    public RGLightUserData() { }
}
