using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindRoomPopup : LobbyPopup
{
    [SerializeField] private Button enterButton;
    [SerializeField] private RectTransform roomPrefabArea;
    [SerializeField] private GameObject waitingRoomPrefab;

    private List<RoomData> waitingRoomList = new List<RoomData>();
    private RoomData selectedRoom = new RoomData();

    protected override void OnEnable()
    {
        base.OnEnable();
        selectedRoom = null;
        FindRoom();
        enterButton.onClick.AddListener(OnClickEnterButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        waitingRoomList.Clear();

        foreach (Transform child in roomPrefabArea)
        {
            Destroy(child.gameObject);
        }

        enterButton.onClick.RemoveAllListeners();
    }

    private async void FindRoom()
    {
        waitingRoomList = await ChatFirebaseManager.Instance.FindRoom();

        if (waitingRoomList.Count < 0)
        {
            Debug.Log("방이 없다");
        }

        foreach (RoomData roomData in waitingRoomList)
        {
            GameObject roomObj = Instantiate(waitingRoomPrefab, roomPrefabArea);
            roomObj.GetComponent<WaitingRoomPrefab>().SetInfo(roomData);
        }
    }

    public void SelectRoom(RoomData roomData)
    {
        selectedRoom = roomData;
    }

    private void OnClickEnterButton()
    {
        if (selectedRoom == null)
        {
            Debug.Log("선택된 방이 없습니다");
        }

        if (selectedRoom != null)
        {
            Debug.Log("선택된 방이 있습니다!");
            //FirebaseManager.Instance.JoinRoom(selectedRoom, FirebaseManager.Instance.CurrentUserData, CheckMatchingSuccess);
        }
    }
}
