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
        print("되나?");
        InputFieldPopup nicknameDecidePopup = PopupManager.Instance.PopupOpen<InputFieldPopup>();

        nicknameDecidePopup.SetPopup("닉네임을 정해주세요", SetNickname);

        _humanButton.onClick.AddListener(() => SetCharacter("human"));
        _ghostButton.onClick.AddListener(() => SetCharacter("ghost"));
        _dokkaebiButton.onClick.AddListener(() => SetCharacter("dokkaebi"));
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

    public async void SetCharacter(string kind)
    {
        if (await FirebaseManager.Instance.SetKind(kind))
        {

        }
        else
        {

        }
    }
}
