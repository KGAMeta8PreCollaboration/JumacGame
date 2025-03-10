using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInPopup : Popup
{
    [SerializeField] private InputField _emailIF;
    [SerializeField] private InputField _passwordIF;

    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _signUpButton;

    [SerializeField] private Text _errorText;

    protected override void OnEnable()
    {
        base.OnEnable();
        _signInButton.onClick.AddListener(SignInButtonClick);
        _signUpButton.onClick.AddListener(SignUpButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _signInButton.onClick.RemoveListener(SignInButtonClick);
        _signUpButton.onClick.RemoveListener(SignUpButtonClick);
    }

    private async void SignInButtonClick()
    {
        if (await GameManager.Instance.LogInManager.SignIn(_emailIF.text, _passwordIF.text))
        {
            _errorText.text = "";
            PopupManager.Instance.PopupClose();

            if (await GameManager.Instance.LogInManager.ExistNicknameAndRace())
            {
                PageManager.Instance.PageOpen<ServerSelectPage>();
            }
            else
            {
                PageManager.Instance.PageOpen<CharaterSelectPage>().OpenNicknamePopup();
            }
        }
        else
        {
            _errorText.text = "로그인할 수 없습니다.";
        }
    }

    private void SignUpButtonClick()
    {
        PopupManager.Instance.PopupOpen<SignUpPopup>();
    }
}
