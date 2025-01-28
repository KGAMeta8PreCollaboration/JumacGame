using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public string nickName;
    public float atk;
    public float def;
    public float hp;
    public float luck;
    public float maxHp;

    [SerializeField] private float stopDistance;
    public GameObject me;
    public GameObject target;

    private Vector3 _myPos;
    private Vector3 _targetPos;

    public bool isAtkEnd = false;
    public bool isDead = false;

    private void Start()
    {
        _myPos = me.transform.position;
        _targetPos = target.transform.position;

        maxHp = hp;

        isAtkEnd = false;
        isDead = false;
    }

    public void DoAttack(CombatUnit target)
    {
        StartCoroutine(StartAttackCoroutine(target));
    }

    private IEnumerator StartAttackCoroutine(CombatUnit target)
    {
        print($"{gameObject.name}의 공격 시작");
        //Vector3 targetPos = target.transform.position;

        //방향
        Vector3 atkDir = (_targetPos - _myPos).normalized;
        //위치
        Vector3 stopPos = _targetPos - (atkDir * stopDistance);

        float movingTime = 0;
        float movingDuration = 1f;
        while (movingTime < movingDuration)
        {
            transform.position = Vector3.Lerp(_myPos, stopPos, movingTime / movingDuration);
            movingTime += Time.deltaTime;
            yield return null;
        }

        //시간 다시 초기화
        movingTime = 0;

        //여기서 공격하는 애니메이션 넣어도 됨
        //gameObject.PlayAttackAnimation();

        float realDamage = CirculateDamage(target);
        target.TakeDamage(realDamage);
        //맞는 애니메이션은 각자 함수에서 실행

        yield return new WaitForSeconds(1f);

        while(movingTime < movingDuration)
        {
            transform.position = Vector3.Lerp(stopPos, _myPos, movingTime / movingDuration);
            movingTime += Time.deltaTime;
            yield return null;
        }
        isAtkEnd = true;
    }
    
    //데미지는 항상 양수가 나와야함
    protected virtual float CirculateDamage(CombatUnit target)
    {
        int probability = Random.Range(0, 100);
        float damage = 0;
        if (luck >= probability)
        {
            //크리티컬
            float criticalDamage = Critical(this.atk);

            if (criticalDamage < target.def)
            {
                return damage = 0;
            }
            else
            {
                return damage = criticalDamage - target.def;
            }
        }
        else
        {
            if (this.atk < target.def)
            {
                return damage = 0;
            }
            else
            {
                return damage = this.atk - target.def;
            }
        }
    }

    //여기에 치명타시 애니메이션 추가 가능
    private float Critical(float damage)
    {
        float criticalDamage = damage * 2;
        return criticalDamage;
    }

    protected virtual void TakeDamage(float damage)
    {
        //hp -= damage;
        //print($"데미지를 받음 {damage}. 남은 체력 : {hp}");

        //float hpAmout = hp / maxHp;
    }

    public virtual void OnDead()
    {
        isDead = true;
        ApplyDeadAnimation();
    }

    protected virtual void ApplyDeadAnimation()
    {
        print($"{nickName}이가 죽음!");
    }
}
