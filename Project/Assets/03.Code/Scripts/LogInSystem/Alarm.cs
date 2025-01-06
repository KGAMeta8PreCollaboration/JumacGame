using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : Popup
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _content;

    [SerializeField] private Button _closeButton;
    private Action _closeAction;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseButtonClick);
    }

    private void CloseButtonClick()
    {
        _closeAction?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetAlarm(string title, string content, Action callback = null)
    {
        _title.text = title;
        _content.text = content;
        _closeAction = callback;
    }
}
