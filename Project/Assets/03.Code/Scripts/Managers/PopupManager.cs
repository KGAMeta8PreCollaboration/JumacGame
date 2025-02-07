using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : Singleton<PopupManager>
{
    [Header("게임 시작 시 Popup 오브젝트가 켜져있어야 찾을 수 있음")]
    [SerializeField] private List<Popup> _popups = new List<Popup>();

    private Stack<Popup> _openPopups = new Stack<Popup>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
        Debug.Log($"Scene Loaded: {scene.name}, PopupManager 초기화.");
    }

    public T PopupOpen<T>() where T : Popup
    {
        T @return = _popups.Find((popup) => popup is T) as T;

        if (@return != null && !_openPopups.Contains(@return))
        {
            @return.transform.SetAsLastSibling();
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
            {
                @return = popup;
                @return.transform.SetAsLastSibling();
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
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        _popups.Clear();

        //현재 씬에 있는 모든 Popup 찾기
        Popup[] foundPopups = GameObject.FindObjectsOfType<Popup>();
        foreach (Popup popup in foundPopups)
        {
            _popups.Add(popup);
        }

        //모든 Popup 비활성화
        foreach (Popup popup in _popups)
        {
            popup.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
