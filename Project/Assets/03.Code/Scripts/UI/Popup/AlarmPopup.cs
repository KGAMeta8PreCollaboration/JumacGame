using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmPopup : Popup
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _contentText;

    public void SetPopup(string title = "", string content = "", Action callback = null)
    {
        _titleText.text = title;
        _contentText.text = content;
        closeAction = callback;
    }
}
