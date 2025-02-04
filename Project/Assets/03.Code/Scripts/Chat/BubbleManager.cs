using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : Singleton<BubbleManager>
{
    [SerializeField] private BubblePrefab bubblePrefab;
    public LocalPlayer _localPlayer;
    public List<RemotePlayer> _remotePlayerList;
    private bool isDone = false;
    private CameraMovement main;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _localPlayer = FindObjectOfType<LocalPlayer>();
    }

    public void MakeMyBubble(MessageData messageData)
    {
        //메시지 보낼때 마다 내 플레이어의 위치를 확인해야함
        _localPlayer = FindObjectOfType<LocalPlayer>();
        Transform parent = _localPlayer.transform;

        BubblePrefab bubble = Instantiate(bubblePrefab, parent);
        bubble.SetText(messageData.Content);
    }

    public void MakeOtherBubble(MessageData messageData)
    {
        FindRemotePlayer();
        _localPlayer = FindObjectOfType<LocalPlayer>();

        foreach (RemotePlayer remotePlayer in _remotePlayerList)
        {
            if (messageData.SenderId == remotePlayer.UID)
            {
                Transform parent = remotePlayer.transform;
                BubblePrefab bubble = Instantiate(bubblePrefab, parent);
                bubble.SetText(messageData.Content);

                //Camera main = _localPlayer.GetComponent<Camera>();

                if (main == null)
                {
                    print("카메라 안달려있음");
                }
                bubble.transform.LookAt(_localPlayer.transform);

                Vector3 euler = bubble.transform.rotation.eulerAngles;
                euler.x = 0f;
                euler.y = 180f;

                bubble.transform.rotation = Quaternion.Euler(euler);
            }
        }

    }

    private void FindRemotePlayer()
    {
        RemotePlayer remotePlayer;

        remotePlayer = FindObjectOfType<RemotePlayer>();
        print($"remotePlayer의 UID : {remotePlayer.UID}");

        _remotePlayerList.Add(remotePlayer);
    }
}
