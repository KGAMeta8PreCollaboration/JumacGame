using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSpawnManager : Singleton<CombatSpawnManager>
{
    [SerializeField] private List<CombatNPCData> combatNPCDataList;

    private LogInUserData _logInUserData;
    private Stat _stat;
    private CombatNPCData _combatNPCData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //클릭 한 후에 정보를 담아줄거임
    public void SetCombatData(LogInUserData logInUserData, Stat stat, CombatNPCData combatNPCdata)
    {
        if (_logInUserData != null || stat != null || _combatNPCData != null)
        {
            _logInUserData = null;
            _stat = null;
            _combatNPCData = null;
        }

        _logInUserData = logInUserData;
        _stat = stat;
        _combatNPCData = combatNPCdata;
    }
}
