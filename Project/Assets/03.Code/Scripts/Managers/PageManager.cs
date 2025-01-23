using LobbyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : Singleton<PageManager>
{
    [Header("안이 비어있다면 Reset을 눌러주세용")]
    [Header("첫번째 배열에는 반드시 맨 처음 열릴 Page가 와야합니다.")]
    [SerializeField] private List<Page> _pages = new List<Page>();

    private void Start()
    {
        PageOpen(0);
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

    public void PageOpen(int index)
    {
        Page selectPage = _pages[index];

        foreach (Page page in _pages)
        {
            if (page == selectPage)
            {
                page.gameObject.SetActive(true);
            }
            else
            {
                page.gameObject.SetActive(false);
            }
        }
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
