using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JegiUIManager : Singleton<JegiUIManager>
{
    [SerializeField] private List<JegiPage> pageList;
    [SerializeField] private List<JegiPopup> popupList;

    public Stack<JegiPopup> openPopupStack = new Stack<JegiPopup>();
    protected override void Awake()
    {
        base.Awake();
        foreach (JegiPage page in pageList)
        {
            page.gameObject.SetActive(false);
        }
        foreach (JegiPopup popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public T PageOpen<T>() where T : JegiPage
    {
        T @return = null;
        foreach (JegiPage page in pageList)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }
        return @return;
    }

    public T PageUse<T>() where T : JegiPage
    {
        T @return = null;
        foreach (JegiPage page in pageList)
        {
            @return = page as T;
        }
        return @return;
    }

    public T PopupOpen<T>() where T : JegiPopup
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
            JegiPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void AllPopupClose()
    {
        while (openPopupStack.Count > 0)
        {
            JegiPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }
}
