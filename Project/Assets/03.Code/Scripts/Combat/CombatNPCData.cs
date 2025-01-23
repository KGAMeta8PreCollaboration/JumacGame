using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatNPCData", menuName = "CombatNPC/CombatNPCData")]
public class CombatNPCData : ScriptableObject
{
    public string npcName;
    public GameObject prefab;
    public float attack;
    public float defense;
    public float health;
    public float luck;
}
