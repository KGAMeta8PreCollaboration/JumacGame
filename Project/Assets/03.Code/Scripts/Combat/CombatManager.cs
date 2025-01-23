using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private List<GameObject> raceList;
    [SerializeField] private Transform leftSpawnPos;
    [SerializeField] private Transform rightSpawnPos;

    public LogInUserData logInUserData;
    public Stat stat;
    public CombatNPCData combatNPCData;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (logInUserData != null || stat != null || combatNPCData != null)
        {
            logInUserData = null;
            stat = null;
            combatNPCData = null;
        }

        logInUserData = CombatSpawnManager.Instance.logInUserData;
        stat = CombatSpawnManager.Instance.stat;
        combatNPCData = CombatSpawnManager.Instance.combatNPCData;

        Init();
    }

    private void Init() 
    {
        InstantiateCombatUnits();
    }

    private void InstantiateCombatUnits()
    {
        foreach (GameObject race in raceList)
        {
            if (race.name == logInUserData.race)
            {
                GameObject leftUnit = Instantiate(race, leftSpawnPos.position, Quaternion.identity);
                leftUnit.transform.rotation = race.transform.localRotation;
            }
        }

        GameObject rightUnitPrefab = combatNPCData.prefab;

        GameObject rightUnit = Instantiate(rightUnitPrefab, rightSpawnPos.position, Quaternion.identity);
        rightUnit.transform.rotation = rightUnitPrefab.transform.localRotation;
    }
}
