using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : Singleton<PopupManager>
{
	[Header("게임 시작 시 Popup 오브젝트가 켜져있어야 찾을 수 있음")]
	[SerializeField] private List<Popup> _popups = new List<Popup>();

	private Transform _canvasPos;
	private Transform _openPoupsPos;
	private Transform _popupsPos;
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
			{
				@return = popup;
				@return.transform.SetParent(_openPoupsPos);
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

	public void Init()
	{
		_popups.Clear();

		Popup[] foundPopups = GameObject.FindObjectsOfType<Popup>();
		foreach (Popup popup in foundPopups)
		{
			_popups.Add(popup);
		}

		_canvasPos = GameObject.Find("Canvas").transform;

		_openPoupsPos = new GameObject("OpenPopups").transform;
		_openPoupsPos.transform.SetParent(_canvasPos);

		_popupsPos = _canvasPos.Find("Popups");
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
