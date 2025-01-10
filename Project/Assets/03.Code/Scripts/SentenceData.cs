using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceData : MonoBehaviour
{
    [SerializeField] private Text text;
    private string _sentence;

    public void SetSentence(string value)
    {
        _sentence = "- " + value;
        text.text = _sentence;
    }
}
