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
        if (await GameManager.Instance.LogInManager.DuplicateNicknameCheck(nickname) == false && nickname.Length >= 2)
        {
            NicknameRaceData data = new NicknameRaceData(nickname);
            PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "사용 가능한 닉네임입니다.", () => NicknameTransfer(data));
        }
        else
        {
            PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("알림", "다시",
                () => PopupManager.Instance.PopupOpen<NicknameSelectPopup>().SetPopup("닉네임을 정해주세요", DuplicateCheck));
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
