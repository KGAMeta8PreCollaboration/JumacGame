using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeOmokRoomPopup : LobbyPopup
{
    [SerializeField] private Button makeRoomButton;
    [SerializeField] private Button findRoomButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        makeRoomButton.onClick.AddListener(OnClickMakeRoomButton);
        findRoomButton.onClick.AddListener(OnClickFindRoomButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        makeRoomButton.onClick.RemoveAllListeners();
        findRoomButton.onClick.RemoveAllListeners();
    }

    private void OnClickMakeRoomButton()
    {
        LobbyTwoInputPopup inputPopup = UILobbyManager.Instance.PopupOpen<LobbyTwoInputPopup>();
        //string roomName = "";
        RoomData newRoom = new RoomData("", 0);
        inputPopup.SetPopup("방 만들기", "방 이름 : ", "배팅금액 : ",
            (roomName) =>
            {
                newRoom.roomName = roomName;
            },
        (betting) =>
        {
            newRoom.betting = betting;
            if (newRoom.roomName == "" || newRoom.betting < 0)
            {
                print("방 정보 입력 안됨");
                UILobbyManager.Instance.PopupOpen<OneButtonPopup>().SetPopup("알림", "방 정보를 입력해주세요");
                return;
            }

            LobbyFirebaseManager.Instance.CreateRoom(newRoom);
            WaitingPopup waitingRoom = UILobbyManager.Instance.PopupOpen<WaitingPopup>();
            waitingRoom.SetWaitingRoom(newRoom, DateTime.Now);
        });


    }

    private void OnClickFindRoomButton()
    {
        UILobbyManager.Instance.PopupOpen<FindRoomPopup>();
    }
}
