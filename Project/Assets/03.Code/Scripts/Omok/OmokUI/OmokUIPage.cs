using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OmokUIPage : OmokPage
{
    [SerializeField] private TextMeshProUGUI turnCountText;
    [SerializeField] private TextMeshProUGUI turnUserName;
    [SerializeField] private RightUserInfo rightUserInfoPrefab;
    [SerializeField] private LeftUserInfo leftUserInfoPrefab;
    [SerializeField] private Button giveUpButton;

    private void OnEnable()
    {
        //Init(OmokFirebaseManager.Instance.currentRoomData);
    }

    private void Awake()
    {
        giveUpButton.onClick.AddListener(OnClickGiveUpButton);
    }

    public void Init(RoomData roomData)
    {
        turnCountText.text = $"{roomData.turnCount + 1}수째.";
        turnUserName.text = $"{OmokFirebaseManager.Instance.hostData.nickname}님의 턴입니다";

        //내가 호스트일때
        if (roomData.host == OmokFirebaseManager.Instance.Auth.CurrentUser.UserId)
        {
            rightUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.hostData, true);
            leftUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.guestData, false);
        }
        else if (roomData.host != OmokFirebaseManager.Instance.Auth.CurrentUser.UserId)
        {
            rightUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.guestData, false);
            leftUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.hostData, true);
        }
    }

    public void UpdateTurnInfo(int turnCount)
    {
        turnCountText.text = $"{turnCount}수째";

        if (turnCount % 2 == 1)
        {
            turnUserName.text = $"{OmokFirebaseManager.Instance.hostData.nickname}님의 턴입니다";
        }
        else if (turnCount % 2 == 0)
        {
            turnUserName.text = $"{OmokFirebaseManager.Instance.guestData.nickname}님의 턴입니다";
        }
    }

    private void OnClickGiveUpButton()
    {

    }
}
