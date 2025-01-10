using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : Singleton<PageManager>
{
	[SerializeField] private List<Page> pages = new List<Page>();

	private void Start()
	{
		PageOpen<TitlePage>();
	}

	public T PageOpen<T>() where T : Page
	{
		T @return = null;
		foreach (Page page in pages)
		{
			bool isActive = page is T;
			page.gameObject.SetActive(isActive);
			if (isActive) @return = page as T;
		}
		return @return;
	}
}
