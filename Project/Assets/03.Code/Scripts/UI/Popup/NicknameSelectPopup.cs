using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameSelectPopup : InputFieldPopup
{
	public Action setNicknameAction;

	public async void DuplicateCheck(string nickname)
	{
		if (await FirebaseManager.Instance.DuplicateNicknameCheck(nickname) == false)
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "사용 가능한 닉네임입니다.", () => SetNickname(nickname));
		}
		else
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "중복된 닉네임입니다.",
				() => PopupManager.Instance.PopupOpen<NicknameSelectPopup>().SetPopup("닉네임을 정해주세요", DuplicateCheck));
		}
	}

	public async void SetNickname(string nickname)
	{
		if (await FirebaseManager.Instance.SetNickname(nickname))
		{
			setNicknameAction?.Invoke();
		}
		else
		{

		}
	}
}
