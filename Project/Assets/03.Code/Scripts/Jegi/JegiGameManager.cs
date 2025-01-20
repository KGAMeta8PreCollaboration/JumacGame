using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class JegiGameManager : Singleton<JegiGameManager>
{
    [SerializeField] private JegiPrefab _jegi;
    [SerializeField] private Transform spawnPoint; //제기 생성할 위치
    
    [SerializeField] private float targetHeight = -3.455088f; //쳐야하는 높이

    private float _targetTime = 0f;

    //올라가는 힘
    [SerializeField] private float perfectForce = 9f;
    [SerializeField] private float greatForce = 7f;
    [SerializeField] private float normalForce = 5f;

    //판정 범위
    [SerializeField] private float perfectRange = 0.2f;
    [SerializeField] private float greatRange = 0.4f;
    [SerializeField] private float normalRange = 0.6f;

    private int _score = 0;
    private bool _isGameOver = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _isGameOver = false;

        //if (_jegi != null)
        //{
        //    Destroy(_jegi);
        //}

        //_jegi = Instantiate(jegiPrefab, spawnPoint.position, Quaternion.identity);
        //_score = 0;

        //_targetTime = Time.time + 1f;

        JegiUIPage jegiPage = JegiUIManager.Instance.PageOpen<JegiUIPage>();
        jegiPage.SetScore(0);
        jegiPage.SetText("");
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // 게임 오버 상태에서는 입력 무시
        //if (_isGameOver) return;

        //입력이 2번 눌려서 추가함
        if (!context.performed) return;

        if (InputSystem.GetDevice<Touchscreen>() != null || Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            print("눌림");
            AttempKick();
        }
    }

    private void AttempKick()
    {
        float jegiY = _jegi.transform.position.y;
        float distanceFromTarget = Mathf.Abs(jegiY - targetHeight);

        string timingResult;
        float forceToAdd;

        if (distanceFromTarget <= perfectRange)
        {
            timingResult = "Perfect";
            forceToAdd = perfectForce;
        }
        else if (distanceFromTarget <= greatRange)
        {
            timingResult = "Great";
            forceToAdd = greatForce;
        }
        else if (distanceFromTarget <= normalRange)
        {
            timingResult = "Normal";
            forceToAdd = normalForce;
        }
        else
        {
            timingResult = "Miss";
            forceToAdd = 0f;
        }

        if (forceToAdd > 0f)
        {
            _jegi.Kick(forceToAdd);
        }

        JegiUIPage jegiPage = JegiUIManager.Instance.PageUse<JegiUIPage>();
        jegiPage.SetText(timingResult);
        
        int addScore = 0;

        switch (timingResult)
        {
            case "Perfect": addScore = 3; break;
            case "Great": addScore = 2; break;
            case "Normal": addScore = 1; break;
        }
        jegiPage.SetScore(addScore);

        //_targetTime = Time.time + 1f;
    }

    public void GameOver()
    {
        if (_isGameOver) return;

        _isGameOver = true;
    }
}
