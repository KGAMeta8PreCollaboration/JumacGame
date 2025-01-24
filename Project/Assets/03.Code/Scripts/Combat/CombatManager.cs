using Minigame.RGLight;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private List<GameObject> raceList;
    [SerializeField] private Transform leftSpawnPos;
    [SerializeField] private Transform rightSpawnPos;

    public LogInUserData logInUserData;
    public Stat stat;
    public CombatNPCData combatNPCData;

    public LeftCombatUnit leftCombatUnit;
    public RightCombatUnit rightCombatUnit;

    public int gold;

    private bool _isBattleEnd = false;
    private bool _isInitEnd = false;
    private bool _isWaitingEnd = false;
    private Coroutine _battleCoroutine;

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

        _isBattleEnd = false;
        _isInitEnd = false;
        _isWaitingEnd = false;
        Init();
    }

    private void Init()
    {
        InstantiateCombatUnits();
        CombatUIManager.Instance.PageOpen<CombatUIPage>().SetPage(leftCombatUnit.nickName, rightCombatUnit.nickName);

        if (_isInitEnd == true)
        {
            StartCoroutine(Temp());
        }
    }

    private IEnumerator Temp()
    {
        StartCoroutine(WaitingTextCoroutine());

        yield return new WaitUntil(() => _isInitEnd == true && _isWaitingEnd == true);
        _battleCoroutine = StartCoroutine(StartBattleCoroutine());
    }

    private void InstantiateCombatUnits()
    {
        foreach (GameObject race in raceList)
        {
            if (race.name == logInUserData.race)
            {
                GameObject leftUnit = Instantiate(race, leftSpawnPos.position, Quaternion.identity);
                leftUnit.transform.rotation = race.transform.localRotation;

                if (leftUnit != null)
                {
                    print("1");
                    LeftCombatUnit tempUnit = leftUnit.GetComponent<LeftCombatUnit>();

                    print("2");
                    tempUnit.nickName = logInUserData.nickname;
                    tempUnit.atk = 10;
                    tempUnit.def = 10;
                    tempUnit.hp = 30;
                    tempUnit.luck = 10;

                    print("3");
                    leftCombatUnit = tempUnit;

                    print($"왼쪽 유닛 지정 완료 : {leftCombatUnit.name}");
                }
                else
                {
                    print("왼쪽 유닛이 생성 안됨");
                }
            }
        }

        GameObject rightUnitPrefab = combatNPCData.prefab;
        gold = combatNPCData.gold;

        if (rightUnitPrefab == null)
        {
            print("오른쪽 프리팹 생성안됨");
            return;
        }

        GameObject rightUnit = Instantiate(rightUnitPrefab, rightSpawnPos.position, Quaternion.identity);
        rightUnit.transform.rotation = rightUnitPrefab.transform.localRotation;

        if (rightUnit != null)
        {
            RightCombatUnit tempUnit = rightUnit.GetComponent<RightCombatUnit>();

            tempUnit.nickName = combatNPCData.npcName;
            tempUnit.atk = combatNPCData.atk;
            tempUnit.def = combatNPCData.def;
            tempUnit.hp = 30;
            tempUnit.luck = combatNPCData.luck;


            rightCombatUnit = tempUnit;

            print($"왼쪽 유닛 지정 완료 : {rightCombatUnit.name}");
        }
        else
        {
            print("오른쪽 유닛이 생성 안됨");
        }

        if (rightCombatUnit != null && leftCombatUnit != null)
        {
            _isInitEnd = true;
        }
    }

    private IEnumerator WaitingTextCoroutine()
    {
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("Ready?");
        yield return new WaitForSeconds(2.5f);
        CombatUIManager.Instance.PopupClose();
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("3");
        yield return new WaitForSeconds(1f);
        CombatUIManager.Instance.PopupClose();
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("2");
        yield return new WaitForSeconds(1f);
        CombatUIManager.Instance.PopupClose();
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("1");
        yield return new WaitForSeconds(1f);
        CombatUIManager.Instance.PopupClose();
        CombatUIManager.Instance.PopupOpen<JudgePopup>().SetJudgeText("Fight!");
        yield return new WaitForSeconds(1f);
        CombatUIManager.Instance.PopupClose();

        _isWaitingEnd = true;
    }

    private IEnumerator StartBattleCoroutine()
    {
        print($"왼쪽 플레이어 체력 : {leftCombatUnit.hp}");
        print($"오른쪽 플레이어 체력 : {rightCombatUnit.hp}");

        while (_isBattleEnd == false)
        {
            leftCombatUnit.DoAttack(rightCombatUnit);

            yield return new WaitUntil(() => leftCombatUnit.isAtkEnd);

            if (rightCombatUnit.hp <= 0)
            {
                rightCombatUnit.OnDead();
                //죽는 함수 실행
                _isBattleEnd = true;
                yield break;
            }

            leftCombatUnit.isAtkEnd = false;

            rightCombatUnit.DoAttack(leftCombatUnit);

            yield return new WaitUntil(() => rightCombatUnit.isAtkEnd);

            if (leftCombatUnit.hp <= 0)
            {
                leftCombatUnit.OnDead();
                //죽는 함수 실행
                _isBattleEnd = true;
                yield break;
            }

            rightCombatUnit.isAtkEnd = false;
        }
    }
}
