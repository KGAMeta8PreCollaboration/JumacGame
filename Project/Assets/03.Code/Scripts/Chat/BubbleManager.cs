using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : Singleton<BubbleManager>
{
    [SerializeField] private BubblePrefab bubblePrefab;
    private LocalPlayer _localPlayer;
    private List<RemotePlayer> _remotePlayer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //_localPlayer = FindObjectOfType<LocalPlayer>();
    }

    public void MakeMyBubble(MessageData messageData)
    {
        //메시지 보낼때 마다 내 플레이어의 위치를 확인해야함
        _localPlayer = FindObjectOfType<LocalPlayer>();
        Transform parent = _localPlayer.transform;
        
        Vector3 localPlayerPos = _localPlayer.transform.position;
        BubblePrefab bubble = Instantiate(bubblePrefab, parent);
        bubble.SetText(messageData.Content);
    }

    public void MakeOtherBubble(MessageData messageData)
    {

    }
}
