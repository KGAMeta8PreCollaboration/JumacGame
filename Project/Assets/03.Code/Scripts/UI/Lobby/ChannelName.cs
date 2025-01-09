using System.Collections;
using TMPro;
using UnityEngine;

public class ChannelName : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private LobbyManager _lobbyManager;
    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();
        StartCoroutine(SetChannelName());
    }
    
    private IEnumerator SetChannelName()
    {
        while (_lobbyManager.logInUserData == null)
        {
            print("waiting for user id");
            yield return new WaitForSeconds(0.2f);
        }
        _text.text = _lobbyManager.logInUserData.serverName;
    }
}
