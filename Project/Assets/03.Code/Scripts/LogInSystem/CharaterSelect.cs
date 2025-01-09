using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : Page
{
	[SerializeField] private Button _humanButton;
	[SerializeField] private Button _ghostButton;
	[SerializeField] private Button _dokkaebiButton;
	[SerializeField] private Button _commitButton;

	private NicknameRaceData _nicknameRaceData;

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
		_commitButton.onClick.AddListener(() => SetNicknameRace(_nicknameRaceData));
	}

	private void OnDisable()
	{
		_humanButton.onClick.RemoveAllListeners();
		_ghostButton.onClick.RemoveAllListeners();
		_dokkaebiButton.onClick.RemoveAllListeners();
		_commitButton.onClick.RemoveAllListeners();
	}

	private void OpenNicknamePopup()
	{
		NicknameSelectPopup nicknameSelectPopup = PopupManager.Instance.PopupOpen<NicknameSelectPopup>();
		nicknameSelectPopup.SetPopup("닉네임을 정해주세요", nicknameSelectPopup.DuplicateCheck);

		nicknameSelectPopup.nickNameTransferAction = (data) =>
		{
			ButtonsInteractive(true);
			_nicknameRaceData = data;
		};
	}

	private void ButtonsInteractive(bool value)
	{
		_humanButton.interactable = value;
		_ghostButton.interactable = value;
		_dokkaebiButton.interactable = value;
		_commitButton.interactable = value;
	}

	public void SetRace(string race)
	{
		_nicknameRaceData.race = race;
	}

	public async void SetNicknameRace(NicknameRaceData data)
	{
		if (data.IsComplete() && data is not null)
		{
			if (await FirebaseManager.Instance.SetNicknameAndRace(data.nickname, data.race))
			{
				PageManager.Instance.PageOpen<ServerSelectPage>();
			}
			else
			{
				PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "데이터 저장에 실패했습니다.");
			}
		}
		else
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "닉네임 또는 캐릭터 정보가 완전하지 않습니다.");
		}
	}
}
