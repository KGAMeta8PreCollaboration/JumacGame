using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyManager : Singleton<UILobbyManager>
{

    [SerializeField] private List<LobbyPage> pageList;
    [SerializeField] private List<LobbyPopup> popupList;
    [SerializeField] private Button popupCloseButton;

    private Stack<LobbyPopup> openPopupStack = new Stack<LobbyPopup>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        popupCloseButton.onClick.AddListener(PopupCloseByTouch);
    }

    private void OnDisable()
    {
        popupCloseButton.onClick.RemoveListener(PopupCloseByTouch);
    }

    private void Start()
    {
        PageOpen<LobbyUIPage>();

        foreach (LobbyPopup popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public T PageOpen<T>() where T : LobbyPage
    {
        T @return = null;
        foreach (LobbyPage page in pageList)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }
        return @return;
    }

    public T PopupOpen<T>() where T : LobbyPopup
    {
        T @return = popupList.Find((popup) => popup is T) as T;
        if (@return != null && !openPopupStack.Contains(@return))
        {
            print($"{@return.name}");
            @return.gameObject.SetActive(true);
            openPopupStack.Push(@return);
        }
        return @return;
    }

    public void PopupClose()
    {
        if (openPopupStack.Count > 0)
        {
            LobbyPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void AllPopupClose()
    {
        while (openPopupStack.Count > 0)
        {
            LobbyPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    private void PopupCloseByTouch()
    {
        print("빈공간 눌리는중");
        PopupClose();
    }
}
