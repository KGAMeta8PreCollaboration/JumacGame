using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatResultPopup : CombatPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI goldText;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void SetPopup(bool amIWin, int gold)
    {
        if (amIWin)
        {
            titleText.text = "승리!";
            goldText.text = $"X {gold}";
        }

        else
        {
            titleText.text = "패배...";
            goldText.text = $"X -{gold}";
        }
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
        SceneManager.LoadScene("Lobby");
    }
}
