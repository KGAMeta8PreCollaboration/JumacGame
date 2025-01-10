using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmokPopup : LobbyPopup
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
        //버튼을 누를때 Popup이 꺼지는 로직을 어떻게 할껀지 정해야한다.
        LobbyInputPopup inputPopup = UILobbyManager.Instance.PopupOpen<LobbyInputPopup>();
        string roomName = "";
        RoomData newRoom = new RoomData(roomName);
        inputPopup.SetPopup("방 만들기", (newRoom) =>
        {
            LobbyFirebaseManager.Instance.CreateRoom(newRoom);
            WaitingPopup waitingRoom = UILobbyManager.Instance.PopupOpen<WaitingPopup>();
            waitingRoom.SetWaitingRoom(newRoom, DateTime.Now);
            print($"방 만들기 성공! 방 이름 : {roomName}");
        }
        );
    }

    private void OnClickFindRoomButton()
    {
        UILobbyManager.Instance.PopupOpen<FindRoomPopup>();
    }
}
