using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPopup : Popup
{
    [SerializeField] private InputField _emailIF;
    [SerializeField] private InputField _passwordIF;
    [SerializeField] private Text _errorText;

    [SerializeField] private Button _signUpButton;

    private void Start()
    {
        _signUpButton.onClick.AddListener(Create);
    }

    public async void Create()
    {
        if (await GameManager.Instance.LogInManager.Create(_emailIF.text, _passwordIF.text))
        {
            PopupManager.Instance.PopupClose();
            PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "계정 생성 완료");
        }
        else
        {
            _errorText.text = "계정을 생성할 수 없습니다.";
        }
    }
}
