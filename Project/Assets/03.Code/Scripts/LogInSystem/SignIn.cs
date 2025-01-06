using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    [SerializeField] private InputField _emailIF;
    [SerializeField] private InputField _passwordIF;

    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private SignUp _signUp;

    [SerializeField] private Text _errorText;

    private void Start()
    {
        _signInButton.onClick.AddListener(LogIn);
        _signUpButton.onClick.AddListener(SignUpButtonClick);
    }

    private async void LogIn()
    {
        if (await FirebaseManager.Instance.SignIn(_emailIF.text, _passwordIF.text))
        {
            print("로그인 성공");
        }
        else
        {
            _errorText.text = "로그인할 수 없습니다.";
        }
    }

    private void SignUpButtonClick()
    {
        _signUp.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
