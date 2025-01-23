using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSpawnManager : Singleton<CombatSpawnManager>
{
    [SerializeField] private List<CombatNPCData> combatNPCDataList;

    public LogInUserData logInUserData;
    public Stat stat;
    public CombatNPCData combatNPCData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //클릭 한 후에 정보를 담아줄거임
    public void SetCombatData(LogInUserData logInUserData, Stat stat, string combatNPCName)
    {
        if (this.logInUserData != null || stat != null || combatNPCData != null)
        {
            this.logInUserData = null;
            this.stat = null;
            combatNPCData = null;
        }

        this.logInUserData = logInUserData;
        this.stat = stat;

        foreach (CombatNPCData combatNPCData in combatNPCDataList)
        {
            if (combatNPCData.name == combatNPCName)
            {
                this.combatNPCData = combatNPCData;
            }
        }
    }
}
