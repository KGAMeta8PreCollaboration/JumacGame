using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OmokUIManager : Singleton<OmokUIManager>
{
    [SerializeField] private List<OmokUIPage> pageList;
    [SerializeField] private List<OmokUIPopup> popupList;

    private Stack<OmokUIPopup> openPopupStack = new Stack<OmokUIPopup>();

    private void Start()
    {
        PageOpen<OmokUIPage>();
    }

    public T PageOpen<T>() where T : OmokUIPage
    {
        T @return = null;
        foreach (OmokUIPage page in pageList)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }

        return @return;
    }

    public T PopupOpen<T>() where T : OmokUIPopup
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
            OmokUIPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void AllPopupClose()
    {
        while (openPopupStack.Count > 0)
        {

            OmokUIPopup targetPopup = openPopupStack.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }
}
