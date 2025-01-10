using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomPrefab : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;
    [SerializeField] private Toggle roomToggle;
    [SerializeField] private Image backgroundImage;
    private ToggleGroup toggleGroup;

    private bool isSelected = false;
    private FindRoomPopup findRoomPopup;
    private RoomData roomData;

    private void Awake()
    {
        isSelected = false;
        toggleGroup = GetComponentInParent<ToggleGroup>();
        findRoomPopup = FindObjectOfType<FindRoomPopup>();
        roomToggle.onValueChanged.AddListener(OnButtonClicked);

        //이게 있어야 하나만 누르는 기능이 가능해짐
        if (toggleGroup != null)
        {
            roomToggle.group = toggleGroup;
        }
    }

    public void SetInfo(RoomData roomData)
    {
        roomNameText.text = roomData.roomName;
        this.roomData = roomData;
    }

    private void OnButtonClicked(bool isOn)
    {
        isSelected = !isSelected;
        UpdateVisualState(isSelected);
    }

    private void UpdateVisualState(bool isSelected)
    {
        if (isSelected)
        {
            backgroundImage.color = Color.gray;
            findRoomPopup.SelectRoom(roomData);
            print($"현재 누르고 있는 방 이름 : {roomData.roomName}");
        }
        else
        {
            backgroundImage.color = Color.white;
        }
    }
}
