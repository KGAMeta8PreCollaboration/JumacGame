using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JegiUIPopup : JegiPopup
{
    [SerializeField] private Button goLobbyButton;
    protected override void OnEnable()
    {
        base.OnEnable();
        goLobbyButton.onClick.AddListener(OnClickGoLobbyButton);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void CloseButtonClick()
    {
        base.CloseButtonClick();
        JegiGameManager.Instance._pause = false;
    }

    private void OnClickGoLobbyButton()
    {
        JegiGameManager.Instance.GoLobby();
    }
}
