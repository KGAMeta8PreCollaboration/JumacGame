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
        print($"스택에 이미 존재 하는 가 :{@return.name} {openPopups.Contains(@return)}");
        if (@return != null && !openPopups.Contains(@return))
        {
            print($"{@return.name}");
            @return.gameObject.SetActive(true);
            openPopups.Push(@return);
        }
        return @return;
    }

    public Popup PopupOpen(string name)
    {
        Popup @return = null;
        foreach (Popup popup in popups)
        {
            if (popup.name.Equals(name))
            {
                popup.gameObject.SetActive(true);
                @return = popup;
                openPopups.Push(@return);
            }
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
