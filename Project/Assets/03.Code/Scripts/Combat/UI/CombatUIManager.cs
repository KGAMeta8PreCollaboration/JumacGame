using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : Singleton<CombatUIManager>
{
    [SerializeField] private List<CombatPage> pageList;
    [SerializeField] private List<CombatPopup> popupList;

    public Stack<CombatPopup> openPopupStack = new Stack<CombatPopup>();

    protected override void Awake()
    {
        base.Awake();
        foreach (CombatPage page in pageList)
        {
            page.gameObject.SetActive(false);
        }
        foreach (CombatPopup popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public T PageOpen<T>() where T : CombatPage
    {
        T @return = null;
        foreach (CombatPage page in pageList)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }
        return @return;
    }

    public T PageUse<T>() where T : CombatPage
    {
        T @return = null;
        foreach (CombatPage page in pageList)
        {
            @return = page as T;
        }
        return @return;
    }

    public T PopupOpen<T>() where T : CombatPopup
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
            CombatPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void AllPopupClose()
    {
        while (openPopupStack.Count > 0)
        {
            CombatPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }
}
