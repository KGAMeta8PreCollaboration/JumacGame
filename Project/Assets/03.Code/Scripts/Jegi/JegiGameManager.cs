using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class JegiGameManager : Singleton<JegiGameManager>
{
    [SerializeField] private JegiPrefab _jegi;
    [SerializeField] private Transform judgeLine;  //판정 라인

    [Header("나중에 지울 거")]
    [SerializeField] private Transform jegiSpawnPos;

    private float _targetHeight; //쳐야하는 높이

    [Header("올라가는 힘")]
    [SerializeField] private float perfectForce = 9f;
    [SerializeField] private float greatForce = 7f;
    [SerializeField] private float normalForce = 5f;

    [Header("판정 범위")]
    [SerializeField] private float perfectRange = 0.2f;
    [SerializeField] private float greatRange = 0.4f;
    [SerializeField] private float normalRange = 0.6f;

    private float angleRangeMin, angleRangeMax;

    private int _score = 0;
    public bool _isGameOver = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _isGameOver = false;

        _targetHeight = judgeLine.position.y;
        JegiUIPage jegiPage = JegiUIManager.Instance.PageOpen<JegiUIPage>();
        jegiPage.SetScore(0);
        jegiPage.SetText("");
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // 게임 오버 상태에서는 입력 무시
        //디버깅중이라 땅에 떨어지는 경우 해제
        if (_isGameOver) return;

        //입력이 2번 눌려서 추가함
        if (!context.performed) return;
        if (_jegi._isKicked == true) return;

        if (InputSystem.GetDevice<Touchscreen>() != null || Mouse.current != null && Mouse.current.leftButton.isPressed)
        {

            print("눌림");
            AttempKick();
        }
    }

    private void AttempKick()
    {
        float jegiY = _jegi.transform.position.y;
        float distanceFromTarget = Mathf.Abs(jegiY - _targetHeight);

        string timingResult; //판정 결과
        float forceToAdd;    //올리는 힘

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
            forceToAdd = -9.81f;
            _isGameOver = true;
        }
        JegiUIPage jegiPage = JegiUIManager.Instance.PageUse<JegiUIPage>();
        jegiPage.SetText(timingResult);

        int addScore = 0;

        switch (timingResult)
        {
            case "Perfect": 
                addScore = 3;
                angleRangeMax = 10f;
                angleRangeMin = -10f;
                break;
            case "Great": 
                addScore = 2;
                angleRangeMax = 30f;
                angleRangeMin = -30f;
                break;
            case "Normal": 
                addScore = 1;
                angleRangeMax = 70f;
                angleRangeMin = -70f;
                break;
            case "Miss":
                addScore = 0;
                angleRangeMax = 0f;
                angleRangeMin = 0f;
                break;
        }

        float randomAngle = Random.Range(angleRangeMin, angleRangeMax);
        if (forceToAdd >= 0f)
        {
            _jegi.Kick(forceToAdd, randomAngle);
        }

        jegiPage.SetScore(addScore);
    }

    public void GameOver()
    {
        if (_isGameOver) return;

        JegiUIPage jegiPage = JegiUIManager.Instance.PageUse<JegiUIPage>();
        jegiPage.SetText("Miss");
        _isGameOver = true;
    }

    public void Restart()
    {
        if (_jegi != null)
        {
            _jegi.transform.position = jegiSpawnPos.transform.position;
            _jegi._rb.velocity = Vector2.zero;
        }
        _isGameOver = false;
    }
}
