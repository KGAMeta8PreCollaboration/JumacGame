using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OmokUIManager : Singleton<OmokUIManager>
{
    [SerializeField] private GameObject leftUserInfoPrefab;
    [SerializeField] private GameObject rightUserInfoPrefab;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
    }

    public void SetUserInfoPrefab(OmokUserData player, OmokUserData opponent)
    {
        GameObject leftUserInfoObj = Instantiate(leftUserInfoPrefab, transform);
        GameObject rightUserInfoObj = Instantiate(rightUserInfoPrefab, transform);

        LeftUserInfo leftUserInfo = leftUserInfoObj.GetComponent<LeftUserInfo>();
        leftUserInfo.SetPrefab(player.nickname, "임시", false);

        RightUserInfo rightUserInfo = rightUserInfoObj.GetComponent<RightUserInfo>();
        rightUserInfo.SetPrefab(opponent.nickname, "임시", true);
    }
}
