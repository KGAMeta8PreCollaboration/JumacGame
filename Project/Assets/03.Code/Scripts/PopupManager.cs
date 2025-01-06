using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private List<Popup> popups = new List<Popup>();

    public T PopupOpen<T>() where T : Popup
    {
        T @return = null;
        foreach (Popup popup in popups)
        {
            bool isActive = popup is T;
            popup.gameObject.SetActive(isActive);
            if (isActive) @return = popup as T;
        }
        return @return;
    }

}
