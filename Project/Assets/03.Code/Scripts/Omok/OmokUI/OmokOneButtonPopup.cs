using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OmokOneButtonPopup : OmokPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI goldText;

    protected override void OnEnable()
    {
        base.OnEnable();
        closeAction += OmokFirebaseManager.Instance.ExitGame;
        Debug.Log("closeAction에 ExitGame 등록됨");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        closeAction -= OmokFirebaseManager.Instance.ExitGame;
        Debug.Log("closeAction에서 ExitGame 제거됨");
    }

    public void AmIWinner(bool amIWin, int gold)
    {
        if (amIWin)
        {
            titleText.text = "승리하였소!";
            contentText.text = "보상";
            goldText.text = $"X {gold}";
        }

        else
        {
            titleText.text = "패배하였소...";
            contentText.text = "보상";
            goldText.text = $"X -{gold}";
        }
    }

    protected override void CloseButtonClick()
    {
        Debug.Log("CloseButton 클릭됨");
        closeAction.Invoke(false);
        base.CloseButtonClick();
        //surrender가 아니니까 fasle를 붙힌다
    }
}
