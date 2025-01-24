using UnityEngine.SceneManagement;

public class JegiInteractable : ButtonInteractable
{
    private void Start()
    {
        buttonName = "제기차러 가기";
    }
    protected override void InteractionButtonClick()
    {
        InteractionInfoPopup infoPopup = PopupManager.Instance.PopupOpen("JegiInfoPopup") as InteractionInfoPopup;
        infoPopup.onStartButtonClick = () => SceneManager.LoadScene("JegiScene");
    }
}
