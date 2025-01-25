using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerSelectPage : Page
{
	[SerializeField] private Button _hanyangButton;
	[SerializeField] private Button _gaeseongButton;
	[SerializeField] private string lobbySceneName;

	[SerializeField] private Text _hanyangPopulation;
	[SerializeField] private Text _gaeseongPopulation;

	[SerializeField] private GameObject _loadingObject;

	private void OnEnable()
	{

		_hanyangButton.onClick.AddListener(() => SetServerName("Hanyang"));
		_gaeseongButton.onClick.AddListener(() => SetServerName("Gaeseong"));
	}

	private void OnDisable()
	{
		_hanyangButton.onClick.RemoveAllListeners();
		_gaeseongButton.onClick.RemoveAllListeners();
	}

	private void Update()
	{
		ServerPopulation();
	}

	private async void ServerPopulation()
	{
		_hanyangPopulation.text = await GameManager.Instance.LogInManager.ServerPopulation("Hanyang");
		_gaeseongPopulation.text = await GameManager.Instance.LogInManager.ServerPopulation("Gaeseong");

	}

	private async void SetServerName(string name)
	{
		if (await GameManager.Instance.LogInManager.SetServerName(name))
		{
			_loadingObject.SetActive(true);
			SceneManager.LoadScene(lobbySceneName);
		}
		else
		{

		}
	}
}
