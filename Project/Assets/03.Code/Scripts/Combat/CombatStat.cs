using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStat
{
    public float atk;
    public float def;
    public float hp;
    public float luck;

    public float plusAtk;
    public float plusDef;
    public float plusHp;
    public float plusLuck;

    public CombatStat(Stat stat)
    {
        atk = stat.atk;
        def = stat.def;
        hp = stat.hp;
        luck = stat.luck;

        plusAtk = stat.plusAtk;
        plusDef = stat.plusDef;
        plusHp = stat.plusHp;
        plusLuck = stat.plusLuck;
    }
}
