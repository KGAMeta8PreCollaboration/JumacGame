using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : Page
{
    [SerializeField] private Button _humanButton;
    [SerializeField] private Button _ghostButton;
    [SerializeField] private Button _dokkaebiButton;

    private void OnEnable()
    {
        InputFieldPopup nicknameSelectPopup = PopupManager.Instance.PopupOpen<InputFieldPopup>();

        nicknameSelectPopup.SetPopup("닉네임을 정해주세요", SetNickname);

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

    public async void SetNickname(string nickname)
    {
        if (await FirebaseManager.Instance.SetNickname(nickname))
        {

        }
        else
        {

        }
    }

    public async void SetRace(string race)
    {
        if (await FirebaseManager.Instance.SetRace(race))
        {

        }
        else
        {

        }
    }
}
