using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [Header("안이 비어있다면 Reset을 눌러주세용")]
    [SerializeField] private List<Popup> _popups = new List<Popup>();

    private Transform _openPoupsPos;
    private Transform _popupsPos;
    private Stack<Popup> _openPopups = new Stack<Popup>();

    private void Start()
    {
        _openPoupsPos = GameObject.Find("OpenPopups").transform;
        _popupsPos = GameObject.Find("Canvas/Popups").transform;
        foreach (Popup popup in _popups)
        {
            popup.gameObject.SetActive(false);
        }
    }

    public T PopupOpen<T>() where T : Popup
    {
        T @return = _popups.Find((popup) => popup is T) as T;
        print($"스택에 이미 존재 하는 가 :{@return.name} {_openPopups.Contains(@return)}");
        if (@return != null && !_openPopups.Contains(@return))
        {
            print($"{@return.name}");
            @return.transform.SetParent(_openPoupsPos);
            @return.gameObject.SetActive(true);
            _openPopups.Push(@return);
        }
        return @return;
    }

    public Popup PopupOpen(string name)
    {
        Popup @return = null;
        foreach (Popup popup in _popups)
        {
            if (popup.name.Equals(name))
                @return.transform.SetParent(_openPoupsPos);
            {
                popup.gameObject.SetActive(true);
                @return = popup;
                _openPopups.Push(@return);
            }
        }
        return @return;
    }

    public void PopupClose()
    {
        if (_openPopups.Count > 0)
        {
            Popup targetPopup = _openPopups.Pop();
            targetPopup.transform.SetParent(_popupsPos);
            targetPopup.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        Popup[] foundPopups = GameObject.FindObjectsOfType<Popup>();
        foreach (Popup popup in foundPopups)
        {
            _popups.Add(popup);
        }
    }

}
