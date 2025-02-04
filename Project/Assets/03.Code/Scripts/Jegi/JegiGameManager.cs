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
    [SerializeField] private Transform _judgeLine;  //판정 라인=

    private float _targetHeight; //쳐야하는 높이

    [Header("올라가는 힘")]
    [SerializeField] private float _perfectForce = 9f;
    [SerializeField] private float _greatForce = 7f;
    [SerializeField] private float _goodForce = 5f;

    [Header("각 판정 범위")]
    [SerializeField] private float _perfectRange = 0.2f;
    [SerializeField] private float _greatRange = 0.4f;
    [SerializeField] private float _goodRange = 0.6f;

    [Header("점수")]
    [SerializeField] private int _perfectScore = 20;
    [SerializeField] private int _greatScore = 10;
    [SerializeField] private int _goodScore = 5;

    [Header("튀는 범위(입력값에 -,+방향)")]
    [SerializeField] private float _perfectAngle = 10;
    [SerializeField] private float _greatAngle = 30;
    [SerializeField] private float _goodAngle = 70;

    [Header("판정 범위")]
    [SerializeField] private float _circleRange = 0.6f;

    [SerializeField] private JudgeTextPrefab _judgeTextPrefab;
    [SerializeField] private RectTransform _judgeTextArea;

    private float _angleRangeMin, _angleRangeMax;
    private JegiUIPage _jegiUIPage;

    private bool _isInitCompelet = false;
    private int _currentScore = 0;
    private int _bestScore;
    public int combo = 0;
    private int _rewardGold = 0;
    public bool pause = true;
    public bool isGameOver = false;

    private void Start()
    {
        _isInitCompelet = false;
    }

    public void Init()
    {
        pause = true;
        isGameOver = false;
        _currentScore = 0;
        _bestScore = JegiFirebaseManager.Instance.jegiUserData.score;
        combo = 0;

        _targetHeight = _judgeLine.position.y;
        _jegiUIPage = FindObjectOfType<JegiUIPage>();
        _isInitCompelet = true;

        AudioManager.Instance.PlayBgm(Bgm.Jegi);
    }

    private void Update()
    {
        if (_isInitCompelet)
        {
            if (pause == true) Time.timeScale = 0;
            else Time.timeScale = 1;

            _jegiUIPage.SetScore(_currentScore, _bestScore);
            _jegiUIPage.SetCombo(combo);
        }
    }

    private Vector2 _worldPos;
    private Vector2 _pointerPos;
    public void OnClick(InputAction.CallbackContext context)
    {
        // 게임 오버 상태에서는 입력 무시
        if (isGameOver) return;

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

        if (distance <= _circleRange)
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
        if (pause == true) return;

        print("AttempKick까지 들어옴");
        float jegiY = _jegi.transform.position.y;
        float distanceFromTarget = Mathf.Abs(jegiY - _targetHeight);

        string timingResult; //판정 결과
        float forceToAdd;    //올리는 힘

        if (distanceFromTarget <= _perfectRange)
        {
            timingResult = "Perfect";
            forceToAdd = _perfectForce;
            AudioManager.Instance.PlaySfx(Sfx.JegiHit);
        }
        else if (distanceFromTarget <= _greatRange)
        {
            timingResult = "Great";
            forceToAdd = _greatForce;
            AudioManager.Instance.PlaySfx(Sfx.JegiHit);
        }
        else if (distanceFromTarget <= _goodRange)
        {
            timingResult = "Good";
            forceToAdd = _goodForce;
            AudioManager.Instance.PlaySfx(Sfx.JegiHit);
        }
        else
        {
            timingResult = "Miss";
            forceToAdd = -9.81f;
            AudioManager.Instance.PlaySfx(Sfx.JegiMiss);

            _jegi._isKicked = true;
            //_isGameOver = true;
        }
        print($"timingResult : {timingResult}");

        switch (timingResult)
        {
            case "Perfect":
                _currentScore += _perfectScore;
                combo += 1;
                _angleRangeMax = _perfectAngle;
                _angleRangeMin = -_perfectAngle;
                break;
            case "Great":
                _currentScore += _greatScore;
                combo += 1;
                _angleRangeMax = _greatAngle;
                _angleRangeMin = -_greatAngle;
                break;
            case "Good":
                _currentScore += _goodScore;
                combo += 1;
                _angleRangeMax = _goodAngle;
                _angleRangeMin = -_goodAngle;
                break;
            case "Miss":
                _currentScore += 0;
                combo = 0;
                _angleRangeMax = 0f;
                _angleRangeMin = 0f;
                break;
        }

        float randomAngle = Random.Range(_angleRangeMin, _angleRangeMax);
        JudgeTextPrefab judgeText = Instantiate(_judgeTextPrefab, _judgeTextArea);
        RectTransform rt = judgeText.GetComponent<RectTransform>();
        rt.position = _pointerPos;

        judgeText.SetPrefab(timingResult);
        print($"{forceToAdd}");
        if (forceToAdd >= 0f)
        {
            _jegi.Kick(forceToAdd, randomAngle);

            judgeText.SetPrefab(timingResult);
        }
    }
    
    public void GameOver()
    {
        if (isGameOver) return;

        AudioManager.Instance.PlaySfx(Sfx.JegiMiss);

        _rewardGold = CalculateReward(_currentScore);
        isGameOver = true;
        pause = true;
        OpenResultPopup();
        JegiFirebaseManager.Instance.UpdateJegiUserData(_currentScore, _rewardGold);
    }

    private void OpenResultPopup()
    {
        JegiResultPopup resultPopup = JegiUIManager.Instance.PopupOpen<JegiResultPopup>();
        resultPopup.SetPopup(_currentScore, combo, _rewardGold);
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
        pause = false;
        SceneManager.LoadScene(nextSceneName);
    }
}


