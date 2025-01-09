using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : Singleton<UILobbyManager>
{

    [SerializeField] private List<Page> pageList;
    [SerializeField] private List<Popup> popupList;

    private Stack<Popup> openPopupStack = new Stack<Popup>();

    private void Start()
    {
        PageOpen<LobbyPage>();

        foreach(Popup popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public T PageOpen<T>() where T : Page
    {
        T @return = null;
        foreach (Page page in pageList)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }
        return @return;
    }

    public T PopupOpen<T>() where T : Popup
    {
        T @return = popupList.Find((popup) => popup is T) as T;
        print($"스택에 이미 존재 하는 가 :{@return.name}{openPopupStack.Contains(@return)}");
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
            Popup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }
}
