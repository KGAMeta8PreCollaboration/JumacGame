using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class NicknameSelectPopup : InputFieldPopup
{
	public Action<NicknameRaceData> nickNameTransferAction;

	public async void DuplicateCheck(string nickname)
	{
		if (await FirebaseManager.Instance.DuplicateNicknameCheck(nickname) == false)
		{
			NicknameRaceData data = new NicknameRaceData(nickname);
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("�˸�", "��� ������ �г����Դϴ�.", () => NicknameTransfer(data));
		}
		else
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("�˸�", "�ߺ��� �г����Դϴ�.",
				() => PopupManager.Instance.PopupOpen<NicknameSelectPopup>().SetPopup("�г����� �����ּ���", DuplicateCheck));
		}
	}

	public void NicknameTransfer(NicknameRaceData data)
	{
		nickNameTransferAction.Invoke(data);
	}
}

[System.Serializable]
public class NicknameRaceData
{
	public string nickname;
	public string race;

	public NicknameRaceData() { }

	public NicknameRaceData(string nickname = default, string race = default)
	{
		this.nickname = nickname;
		this.race = race;
	}

	public bool IsComplete()
	{
		return !string.IsNullOrEmpty(nickname) && !string.IsNullOrEmpty(race);
	}
}
