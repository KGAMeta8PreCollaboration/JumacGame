using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatNPCData", menuName = "CombatNPC/CombatNPCData")]
public class CombatNPCData : ScriptableObject
{
    public string npcName;
    public GameObject prefab;
    public float atk;
    public float def;
    public float hp;
    public float luck;
    public int gold;
}
