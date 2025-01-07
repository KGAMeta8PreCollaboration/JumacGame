using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private List<Popup> popups = new List<Popup>();

    private Stack<Popup> openPopups = new Stack<Popup>();

    private void Start()
    {
        foreach (Popup popup in popups)
        {
            popup.gameObject.SetActive(false);
        }

    }

    public T PopupOpen<T>() where T : Popup
    {
        T @return = popups.Find((popup) => popup is T) as T;
        if (@return != null && !openPopups.Contains(@return))
        {
            @return.gameObject.SetActive(true);
            openPopups.Push(@return);
        }
        return @return;
    }

    public void PopupClose()
    {
        if (openPopups.Count > 0)
        {
            Popup targetPopup = openPopups.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

}
