using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerSelectPage : Page
{
    [SerializeField] private Button _hanyangButton;
    [SerializeField] private Button _gaeseongButton;

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

    private async void SetServerName(string name)
    {
        if (await FirebaseManager.Instance.SetServerName(name))
        {
            SceneManager.LoadScene("SEOLobbyTest");
        }
        else
        {

        }
    }
}
