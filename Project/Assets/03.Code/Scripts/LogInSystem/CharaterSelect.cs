using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : Page
{
	[SerializeField] private Button _humanButton;
	[SerializeField] private Button _ghostButton;
	[SerializeField] private Button _dokkaebiButton;

	private void Awake()
	{
		ButtonsInteractive(false);
	}

	private void OnEnable()
	{
		OpenNicknamePopup();

		_humanButton.onClick.AddListener(() => SetRace("human"));
		_ghostButton.onClick.AddListener(() => SetRace("ghost"));
		_dokkaebiButton.onClick.AddListener(() => SetRace("dokkaebi"));
	}

	private void OnDisable()
	{
		_humanButton.onClick.RemoveAllListeners();
		_ghostButton.onClick.RemoveAllListeners();
		_dokkaebiButton.onClick.RemoveAllListeners();
	}

	private void OpenNicknamePopup()
	{
		NicknameSelectPopup nicknameSelectPopup = PopupManager.Instance.PopupOpen<NicknameSelectPopup>();

		nicknameSelectPopup.SetPopup("닉네임을 정해주세요", nicknameSelectPopup.DuplicateCheck);

		nicknameSelectPopup.setNicknameAction = () => ButtonsInteractive(true);
	}

	private void ButtonsInteractive(bool value)
	{
		_humanButton.interactable = value;
		_ghostButton.interactable = value;
		_dokkaebiButton.interactable = value;
	}

	public async void SetRace(string race)
	{
		if (await FirebaseManager.Instance.SetRace(race))
		{
			PageManager.Instance.PageOpen<ServerSelectPage>();
		}
		else
		{

		}
	}
}
