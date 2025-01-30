using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeftCombatUnit : CombatUnit
{
    public LeftCombatUnit(LogInUserData logInUserData, CombatStat combatStat)
    {
        this.nickName = logInUserData.nickname;
        this.atk = combatStat.atk + combatStat.plusAtk;
        this.def = combatStat.def + combatStat.plusDef;
        this.hp = combatStat.hp + combatStat.plusHp;
        this.luck = combatStat.luck + combatStat.plusLuck;
    }

    protected override void TakeDamage(float damage)
    {
        hp -= damage;
        print($"{nickName}이 가데미지를 받음 {damage}. 남은 체력 : {hp}");

        float hpAmout = hp / maxHp;
        CombatUIManager.Instance.PageUse<CombatUIPage>().SetLeftHpBar(hpAmout);

        DamageTextPrefab damageObj = Instantiate(damageTextPrefab, transform);

        Vector3 worldPos = transform.position + Vector3.up * 1.0f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        damageObj.damageText.GetComponent<RectTransform>().position = screenPos;

        damageObj.SetDamageText(damage);
        //Vector3 spawnPos = me.transform.position + new Vector3(0.5f, 0.7f, -1.5f);
        //damageText.transform.position = spawnPos;
        //damageText.SetDamageText(damage);
    }

    //왼쪽 플레이어가 죽으면 진거임
    public override void OnDead()
    {
        base.OnDead();
        StartCoroutine(HandleWin());
    }

    private IEnumerator HandleWin()
    {
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("YOU LOSE");
        yield return new WaitForSeconds(2f);

        AudioManager.Instance.PlaySfx(Sfx.BattleLose);

        //골드량은 나중에 CombatManager에서 가져오자
        CombatUIManager.Instance.PopupOpen<CombatResultPopup>().SetPopup(false, CombatManager.Instance.gold);
    }
}
