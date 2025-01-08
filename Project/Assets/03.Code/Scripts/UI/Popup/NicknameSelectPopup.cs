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
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("�˸�", "��� ������ �г����Դϴ�.", () => SetNickname(nickname));
		}
		else
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("�˸�", "�ߺ��� �г����Դϴ�.",
				() => PopupManager.Instance.PopupOpen<NicknameSelectPopup>().SetPopup("�г����� �����ּ���", DuplicateCheck));
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
