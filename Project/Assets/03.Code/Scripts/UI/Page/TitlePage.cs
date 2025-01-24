using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : Page
{
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;

    private void OnEnable()
    {
        signInButton.onClick.AddListener(SignInButtonClick);
        signUpButton.onClick.AddListener(SignUpButtonClick);
    }

    private void Start()
    {
        AudioManager.Instance.PlayBgm(Bgm.Title);
    }

    private void OnDisable()
    {
        signInButton.onClick.RemoveListener(SignInButtonClick);
        signUpButton.onClick.RemoveListener(SignUpButtonClick);
    }

    private void SignInButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.ButtonPress);
        PopupManager.Instance.PopupOpen<SignInPopup>();
    }

    private void SignUpButtonClick()
    {
        PopupManager.Instance.PopupOpen<SignUpPopup>();
    }
}