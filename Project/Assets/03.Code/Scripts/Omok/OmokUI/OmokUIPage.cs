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

    public void Init(RoomData roomData, OmokUserData hostData, OmokUserData guestData, bool amIHost)
    {
        try
        {
            turnCountText.text = $"{roomData.turnCount + 1}수째.";

            //내가 호스트일때
            if (amIHost)
            {
                rightUserInfoPrefab.SetPrefab(hostData, true);
                leftUserInfoPrefab.SetPrefab(guestData, false);
                turnUserName.text = $"당신의 턴입니다";
            }
            else
            {
                rightUserInfoPrefab.SetPrefab(guestData, false);
                leftUserInfoPrefab.SetPrefab(hostData, true);
                turnUserName.text = $"상대의 턴입니다";
            }

            //if (roomData.host == OmokFirebaseManager.Instance.Auth.CurrentUser.UserId)
            //{
            //    //오른쪽에는 호스트(나)의 정보, 돌의 색은 = true
            //    rightUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.hostData, amIHost);
            //    leftUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.guestData, !amIHost);
            //}
            //else if (roomData.host != OmokFirebaseManager.Instance.Auth.CurrentUser.UserId)
            //{
            //    rightUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.guestData, !amIHost);
            //    leftUserInfoPrefab.SetPrefab(OmokFirebaseManager.Instance.hostData, amIHost);
            //}
        }
        catch (Exception e)
        {
            Debug.LogWarning($"UI로딩 오류 : {e.Message}");
        }
    }

    public void UpdateTurnInfo(int turnCount, bool isMyTurn)
    {
        turnCountText.text = $"{turnCount + 1}수째";

        if (isMyTurn)
        {
            turnUserName.text = $"당신의 턴입니다";
        }
        else
        {
            turnUserName.text = $"상대의 턴입니다";
        }
    }

    private void OnClickGiveUpButton()
    {
        OmokUIManager.Instance.PopupOpen<OmokTwoButtonPopup>().SetPopup("정말 기권하시겠습니까?", OmokFirebaseManager.Instance.ExitGame);
    }
}
