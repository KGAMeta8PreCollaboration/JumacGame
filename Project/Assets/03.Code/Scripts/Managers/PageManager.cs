using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : Singleton<PageManager>
{
    [Header("안이 비어있다면 Reset을 눌러주세용")]
    [SerializeField] private List<Page> _pages = new List<Page>();

    private void Start()
    {
        PageOpen<TitlePage>();
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

    private void Reset()
    {
        Page[] foundPages = GameObject.FindObjectsOfType<Page>();
        foreach (Page page in foundPages)
        {
            _pages.Add(page);
        }
    }
}
