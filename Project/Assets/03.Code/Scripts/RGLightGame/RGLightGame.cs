using Minigame.RGLight;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RGLightGame : MonoBehaviour
{
    private TextMeshProUGUI _charText;
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
                _charText.text = c.ToString();
                yield return new WaitForSeconds(blankInterval);
            }
            else
            {
                print(c);
                _charText.text = c.ToString();
                yield return new WaitForSeconds(notBlankInterval);
            }
        }

        _charText.text = "";
        if (!RGLightManager.IsEndGame) endSentenceAction?.Invoke();
    }

    public void Init(RGLightManager manager)
    {
        RGLightManager = manager;
        _charText = GameObject.Find("Char").GetComponent<TextMeshProUGUI>();
    }
}
