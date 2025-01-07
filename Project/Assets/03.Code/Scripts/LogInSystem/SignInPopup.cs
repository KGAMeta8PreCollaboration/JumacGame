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
		_signInButton.onClick.AddListener(SignIn);
		_signUpButton.onClick.AddListener(SignUpButtonClick);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_signInButton.onClick.RemoveListener(SignIn);
		_signUpButton.onClick.RemoveListener(SignUpButtonClick);
	}

	private async void SignIn()
	{
		if (await FirebaseManager.Instance.SignIn(_emailIF.text, _passwordIF.text))
		{
			_errorText.text = "";
			PageManager.Instance.PageOpen<CharaterSelect>();
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
