using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneButtonPopup : LobbyPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    public void SetPopup(string titleText, string contentText)
    {
        this.titleText.text = titleText;
        this.contentText.text = contentText;
    }
}
