using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitlePage : Page, IPointerClickHandler
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _signUpButton;

    [SerializeField] private GameObject _buttonsObject;
    [SerializeField] private Text _IntroText;

    private void Start()
    {
        AudioManager.Instance.PlayBgm(Bgm.Title);
        _signInButton.onClick.AddListener(SignInButtonClick);
        _signUpButton.onClick.AddListener(SignUpButtonClick);
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveListener(SignInButtonClick);
        _signUpButton.onClick.RemoveListener(SignUpButtonClick);
    }

    private void SignInButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.ButtonPress);
        PopupManager.Instance.PopupOpen<SignInPopup>();
    }

    private void SignUpButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.ButtonPress);
        PopupManager.Instance.PopupOpen<SignUpPopup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _IntroText.gameObject.SetActive(false);
        _buttonsObject.SetActive(true);
    }
}