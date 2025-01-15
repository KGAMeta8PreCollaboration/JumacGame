using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OmokUIManager : Singleton<OmokUIManager>
{
    [SerializeField] private List<OmokPage> pageList;
    [SerializeField] private List<OmokPopup> popupList;

    private Stack<OmokPopup> openPopupStack = new Stack<OmokPopup>();
    protected override void Awake()
    {
        base.Awake();
        foreach (OmokPage page in pageList)
        {
            page.gameObject.SetActive(false);
        }
        foreach (OmokPopup popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public T PageOpen<T>() where T : OmokPage
    {
        T @return = null;
        foreach (OmokPage page in pageList)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }
        return @return;
    }

    public T PageUse<T>() where T : OmokPage
    {
        T @return = null;
        foreach (OmokPage page in pageList)
        {
            @return = page as T;
        }
        return @return;
    }

    public T PopupOpen<T>() where T : OmokPopup
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
            OmokPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void AllPopupClose()
    {
        while (openPopupStack.Count > 0)
        {
            OmokPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }
}
