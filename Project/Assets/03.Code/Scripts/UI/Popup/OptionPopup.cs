using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : Popup
{
	private Button _soundButton;
	private Button _horrorButton;
	private Button _signOutButton;

	private void Awake()
	{
		_soundButton = transform.Find("Buttons/SoundButton").GetComponent<Button>();
		_horrorButton = transform.Find("Buttons/HorrorButton").GetComponent<Button>();
		_signOutButton = transform.Find("Buttons/SignOutButton").GetComponent<Button>();
	}

	private void Start()
	{
		_soundButton.onClick.AddListener(SoundButtonClick);
		_horrorButton.onClick.AddListener(HorrorButtonClick);
		_signOutButton.onClick.AddListener(SignOutButtonClick);
	}

	private void SoundButtonClick()
	{
		PopupManager.Instance.PopupOpen<SoundSettingsPopup>();
	}

	private void HorrorButtonClick()
	{

	}

	private void SignOutButtonClick()
	{
		GameManager.Instance.LogInManager.SignOut();
	}

	private void OnDestroy()
	{
		_soundButton.onClick.RemoveListener(SoundButtonClick);
		_horrorButton.onClick.RemoveListener(HorrorButtonClick);
		_signOutButton.onClick.RemoveListener(SignOutButtonClick);
	}
}
