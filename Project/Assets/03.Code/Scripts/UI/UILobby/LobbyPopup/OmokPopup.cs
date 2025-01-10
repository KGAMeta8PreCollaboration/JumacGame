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
        //��ư�� ������ Popup�� ������ ������ ��� �Ҳ��� ���ؾ��Ѵ�.
        LobbyInputPopup inputPopup = UILobbyManager.Instance.PopupOpen<LobbyInputPopup>();
        string roomName = "";
        RoomData newRoom = new RoomData(roomName);
        inputPopup.SetPopup("�� �����", (newRoom) =>
        {
            LobbyFirebaseManager.Instance.CreateRoom(newRoom);
            WaitingPopup waitingRoom = UILobbyManager.Instance.PopupOpen<WaitingPopup>();
            waitingRoom.SetWaitingRoom(newRoom, DateTime.Now);
            print($"�� ����� ����! �� �̸� : {roomName}");
        }
        );
    }

    private void OnClickFindRoomButton()
    {
        UILobbyManager.Instance.PopupOpen<FindRoomPopup>();
    }
}
