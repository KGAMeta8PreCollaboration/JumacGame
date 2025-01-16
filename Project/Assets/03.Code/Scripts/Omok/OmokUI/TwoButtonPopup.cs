using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TwoButtonPopup : OmokPopup
{
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button yesButton;
    private Action _callback;

    protected override void OnEnable()
    {
        base.OnEnable();
        yesButton.onClick.AddListener(OnClickYesButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        yesButton.onClick.RemoveAllListeners();
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
    }

    public void SetPopup(string content, Action callback)
    {
        this.contentText.text = content;
        this._callback = callback;
    }

    private void OnClickYesButton()
    {
        _callback?.Invoke();
        OmokUIManager.Instance.PopupClose();
    }
}
