using LobbyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageManager : Singleton<PageManager>
{
    [Header("게임 시작 시 Page 오브젝트가 켜져있어야 찾을 수 있음")]
    [SerializeField] private List<Page> _pages = new List<Page>();

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
        Debug.Log($"Scene Loaded: {scene.name}, PageManager 초기화.");
    }

    public T PageOpen<T>() where T : Page
    {
        T @return = null;
        foreach (Page page in _pages)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) @return = page as T;
        }
        return @return;
    }

    private void Init()
    {
        _pages.Clear();
        Page[] foundPages = GameObject.FindObjectsOfType<Page>();
        foreach (Page page in foundPages)
        {
            _pages.Add(page);
            page.gameObject.SetActive(page.IsDefault);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
