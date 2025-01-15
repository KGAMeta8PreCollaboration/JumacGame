using Minigame.RGLight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGLightGame : MonoBehaviour
{
    public string sentence;
    public float blankInterval;
    public float notBlankInterval;
    public Action endSentenceAction;

    public RGLightManager RGLightManager { get; private set; }

    public IEnumerator ControllReadSentence()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(ReadSentence());
    }

    public IEnumerator ReadSentence()
    {
        char[] array = sentence.ToCharArray();
        foreach (char c in array)
        {
            if (c.Equals(' '))
            {
                print(c);
                yield return new WaitForSeconds(blankInterval);
            }
            else
            {
                print(c);
                yield return new WaitForSeconds(notBlankInterval);
            }
        }

        if(!RGLightManager.IsEndGame) endSentenceAction?.Invoke();
    }

    public void Init(RGLightManager manager)
    {
        RGLightManager = manager;
    }
}
