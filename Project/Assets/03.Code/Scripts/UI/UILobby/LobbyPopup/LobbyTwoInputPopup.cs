using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTwoInputPopup : LobbyPopup
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TMP_InputField firstInput;
    [SerializeField] private TMP_InputField secondInput;
    [SerializeField] private Button confirmButton;
    private Action<string> _firstCallback;
    private Action<int> _secondCallback;

    protected override void OnEnable()
    {
        base.OnEnable();
        confirmButton.onClick.AddListener(OnClickConfirmButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        confirmButton.onClick.RemoveAllListeners();
    }

    public void SetPopup(string titleText, string firstText, string secondText, Action<string> firstCallback, Action<int> secondCallback)
    {
        this.titleText.text = titleText;
        this.firstText.text = firstText;
        this.secondText.text = secondText;
        _firstCallback = firstCallback;
        _secondCallback = secondCallback;
    }

    //방 정보를 입력하고 확인 버튼 누르는 부분
    private void OnClickConfirmButton()
    {
        _firstCallback.Invoke(firstInput.text);

        int betValue;
        if (!int.TryParse(secondInput.text, out betValue))
        {
            UILobbyManager.Instance.PopupOpen<OneButtonPopup>().SetPopup("알림", "배팅금은 숫자로 입력해주세요");
            return;
        }
        else
        {
            //나중에 OmokUserData를 들고오면 골드 확인해보자
            //if (betValue >= LobbyFirebaseManager.Instance.gameUserData.gold || betValue < 0)
            //{
            //    //예외처리
            //}

            UILobbyManager.Instance.PopupClose();
            _secondCallback.Invoke(int.Parse(secondInput.text));

        }
    }
}