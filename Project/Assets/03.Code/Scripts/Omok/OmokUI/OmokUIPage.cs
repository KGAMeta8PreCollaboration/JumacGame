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

    private void Awake()
    {
        giveUpButton.onClick.AddListener(OnClickGiveUpButton);
    }

    public void Init(RoomData roomData)
    {
        turnCountText.text = $"{roomData.turnCount + 1}수째.";
        turnUserName.text = $"{OmokFirebaseManager.Instance.hostData.nickname}님의 턴입니다";

        bool isBlackStone = true;
        //내가 호스트일때
        if (roomData.host == OmokFirebaseManager.Instance.Auth.CurrentUser.UserId)
        {
            //오른쪽에는 호스트(나)의 정보, 돌의 색은 = true
            rightUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.hostData, isBlackStone);
            leftUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.guestData, !isBlackStone);
        }
        else if (roomData.host != OmokFirebaseManager.Instance.Auth.CurrentUser.UserId)
        {
            rightUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.guestData, !isBlackStone);
            leftUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.hostData, isBlackStone);
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
        OmokUIManager.Instance.PopupOpen<OmokTwoButtonPopup>().SetPopup("정말 기권하시겠습니까?", OmokFirebaseManager.Instance.ExitGame);
    }
}
