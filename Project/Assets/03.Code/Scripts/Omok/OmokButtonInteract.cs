using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmokButtonInteract : ButtonInteractable
{
    protected override void InteractionButtonClick()
    {
        UILobbyManager.Instance.PopupOpen<MakeOmokRoomPopup>();
    }
}
