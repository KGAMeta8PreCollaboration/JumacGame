using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LogInManager _logInManagerPrefab;
    [SerializeField] private FirebaseManager _firebaseManagerPrefab;
    public LogInManager LogInManager { get; private set; }
    public FirebaseManager FirebaseManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        LogInManager = Instantiate(_logInManagerPrefab, transform);
        FirebaseManager = Instantiate(_firebaseManagerPrefab, transform);
    }
}
