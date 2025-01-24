using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCombatUnit : CombatUnit
{
    public RightCombatUnit(string nickName, float atk, float def, float hp, float luck)
    {
        this.nickName = nickName;
        this.atk = atk;
        this.def = def;
        this.hp = 10000;
        this.luck = luck;
    }

    protected override void TakeDamage(float damage)
    {
        hp -= damage;
        print($"{nickName}이 가데미지를 받음 {damage}. 남은 체력 : {hp}");

        float hpAmout = hp / maxHp;
        CombatUIManager.Instance.PageUse<CombatUIPage>().SetRightHpBar(hpAmout);
    }

    public override void OnDead()
    {
        base.OnDead();
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("YOU WIN!");

        CombatUIManager.Instance.PopupOpen<CombatResultPopup>().SetPopup(true, CombatManager.Instance.gold);
    }
}
