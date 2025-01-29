using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        DamageTextPrefab damageObj = Instantiate(damageTextPrefab, transform);

        Vector3 worldPos = transform.position + Vector3.up * 1.0f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        damageObj.damageText.GetComponent<RectTransform>().position = screenPos;

        damageObj.SetDamageText(damage);
        //DamageTextPrefab damageText = Instantiate(damageTextPrefab, transform);

        //Vector3 spawnPos = me.transform.position + new Vector3(0.5f, 0.7f, -1.5f);
        //damageText.transform.position = spawnPos;
        //damageText.SetDamageText(damage);
    }

    public override void OnDead()
    {
        base.OnDead();
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("YOU WIN!");

        CombatUIManager.Instance.PopupOpen<CombatResultPopup>().SetPopup(true, CombatManager.Instance.gold);
    }
}
