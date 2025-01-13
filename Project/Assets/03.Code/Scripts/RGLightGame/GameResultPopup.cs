using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPopup : Popup
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _durationTimeText;
    [SerializeField] private Text _moneyText;

    public void SetPopup(string title, string durationTime, int money, Action action = null)
    {
        _titleText.text = title;
        _durationTimeText.text = durationTime;
        _moneyText.text = money.ToString();
        closeAction?.Invoke();
    }
}
