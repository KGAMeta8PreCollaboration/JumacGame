using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private float goodForce = 5f;

    [Header("각 판정 범위")]
    [SerializeField] private float perfectRange = 0.2f;
    [SerializeField] private float greatRange = 0.4f;
    [SerializeField] private float goodRange = 0.6f;

    [Header("점수")]
    [SerializeField] private int perfectScore = 20;
    [SerializeField] private int greatScore = 10;
    [SerializeField] private int goodScore = 5;

    [Header("튀는 범위(입력값에 -,+방향)")]
    [SerializeField] private float perfectAngle = 10;
    [SerializeField] private int greatAngle = 30;
    [SerializeField] private int goodAngle = 70;

    [Header("판정 범위")]
    [SerializeField] private float circleRange = 0.6f;

    [SerializeField] private JudgeTextPrefab judgePrefab;
    [SerializeField] private RectTransform judgeTextArea;

    private float angleRangeMin, angleRangeMax;
    private JegiUIPage _jegiUIPage;

    private bool _isInitCompelet = false;
    private int _currentScore = 0;
    private int _bestScore;
    public int _combo = 0;
    private int _rewardGold = 0;
    public bool _pause = true;
    public bool _isGameOver = false;

    private void Start()
    {
        _isInitCompelet = false;
    }

    public void Init()
    {
        _pause = true;
        _isGameOver = false;
        _currentScore = 0;
        _bestScore = JegiFirebaseManager.Instance.jegiUserData.score;
        _combo = 0;

        _targetHeight = judgeLine.position.y;
        _jegiUIPage = FindObjectOfType<JegiUIPage>();
        _isInitCompelet = true;
    }

    private void Update()
    {
        if (_isInitCompelet)
        {
            if (_pause == true) Time.timeScale = 0;
            else Time.timeScale = 1;

            _jegiUIPage.SetScore(_currentScore, _bestScore);
            _jegiUIPage.SetCombo(_combo);
        }
    }

    private Vector2 _worldPos;
    private Vector2 _pointerPos;
    public void OnClick(InputAction.CallbackContext context)
    {
        // 게임 오버 상태에서는 입력 무시
        if (_isGameOver) return;

        //입력이 2번 눌려서 추가함
        if (!context.performed) return;
        if (_jegi._isKicked == true) return;

        _pointerPos = GetInputPosition();
        print($"마우스 좌표 : {_pointerPos}");

        Vector3 screenPoint = new Vector3(_pointerPos.x, _pointerPos.y, -Camera.main.transform.position.z);
        _worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
        print($"클릭된 worldPos : {_worldPos}");
        
        if (IsHit(_worldPos) == true)
        {
            AttempKick();
        }
        else
        {
            print("판정범위 밖");
        }
    }

    private Vector2 GetInputPosition()
    {
        if (InputSystem.GetDevice<Touchscreen>() != null)
        {
            print("클릭됨");
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            print("클릭됨");
            return Mouse.current.position.ReadValue();
        }

        return Vector2.zero;
    }

    private bool IsHit(Vector2 pointerPos)
    {
        Vector2 jegiPos = _jegi.transform.position;
        print($"jegi의 포지션 : {jegiPos}");

        float distance = Vector2.Distance(pointerPos, jegiPos);

        if (distance <= circleRange)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void AttempKick()
    {
        if (_pause == true) return;

        print("AttempKick까지 들어옴");
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
        else if (distanceFromTarget <= goodRange)
        {
            timingResult = "Good";
            forceToAdd = goodForce;
        }
        else
        {
            timingResult = "Miss";
            forceToAdd = -9.81f;

            _jegi._isKicked = true;
            //_isGameOver = true;
        }
        print($"timingResult : {timingResult}");

        switch (timingResult)
        {
            case "Perfect":
                _currentScore += perfectScore;
                _combo += 1;
                angleRangeMax = perfectAngle;
                angleRangeMin = -perfectAngle;
                break;
            case "Great":
                _currentScore += greatScore;
                _combo += 1;
                angleRangeMax = greatAngle;
                angleRangeMin = -greatAngle;
                break;
            case "Good":
                _currentScore += goodScore;
                _combo += 1;
                angleRangeMax = goodAngle;
                angleRangeMin = -goodAngle;
                break;
            case "Miss":
                _currentScore += 0;
                _combo = 0;
                angleRangeMax = 0f;
                angleRangeMin = 0f;
                break;
        }

        float randomAngle = Random.Range(angleRangeMin, angleRangeMax);
        JudgeTextPrefab judgeText = Instantiate(judgePrefab, judgeTextArea);
        RectTransform rt = judgeText.GetComponent<RectTransform>();
        rt.position = _pointerPos;

        judgeText.SetPrefab(timingResult);
        print($"{forceToAdd}");
        if (forceToAdd >= 0f)
        {
            _jegi.Kick(forceToAdd, randomAngle);

            //JudgeTextPrefab judgeText = Instantiate(judgePrefab, judgeTextArea);
            //RectTransform rt = judgeText.GetComponent<RectTransform>();
            //rt.position = _pointerPos;

            judgeText.SetPrefab(timingResult);
        }
    }
    
    public void GameOver()
    {
        if (_isGameOver) return;

        _rewardGold = CalculateReward(_currentScore);
        _isGameOver = true;
        _pause = true;
        OpenResultPopup();
        JegiFirebaseManager.Instance.UpdateJegiUserData(_currentScore, _rewardGold);
    }

    public void Restart()
    {
        if (_jegi != null)
        {
            _jegi.transform.position = jegiSpawnPos.transform.position;
            _jegi._rb.velocity = Vector2.zero;
        }
        _isGameOver = false;
        _jegi._isKicked = false;
        JegiUIManager.Instance.PageUse<JegiUIPage>().SetStartButton();
    }

    private void OpenResultPopup()
    {
        JegiResultPopup resultPopup = JegiUIManager.Instance.PopupOpen<JegiResultPopup>();
        resultPopup.SetPopup(_currentScore, _combo, _rewardGold);
    }

    private int CalculateReward(int score)
    {
        int reward = 0;
        reward = score / 10;

        return reward;
    }

    public string nextSceneName = "YooLobby";
    
    public void GoLobby()
    {
        _pause = false;
        SceneManager.LoadScene(nextSceneName);
    }
}


