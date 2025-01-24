using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmokButtonInteract : ButtonInteractable
{
    public string nextScene;
    private void Start()
    {
        buttonName = "오목하러가기";
    }
    protected override void InteractionButtonClick()
    {
        // UILobbyManager.Instance.PopupOpen<MakeOmokRoomPopup>();
        InteractionInfoPopup infoPopup = PopupManager.Instance.PopupOpen("OmokInfoPopup") as InteractionInfoPopup;
        infoPopup.onStartButtonClick = () => UILobbyManager.Instance.PopupOpen<MakeOmokRoomPopup>();
    }
}
