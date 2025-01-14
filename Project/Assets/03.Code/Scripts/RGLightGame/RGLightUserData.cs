using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RGLightUserData
{
	public int score;

	public RGLightUserData(int score = default)
	{
		this.score = score;
	}

	public RGLightUserData() { }
}
