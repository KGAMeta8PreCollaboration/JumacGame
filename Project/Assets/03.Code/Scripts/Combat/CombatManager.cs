using Minigame.RGLight;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private List<GameObject> raceList;
    [SerializeField] private Transform leftSpawnPos;
    [SerializeField] private Transform rightSpawnPos;

    public LogInUserData logInUserData;
    public CombatStat combatStat;
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
        if (logInUserData != null || combatStat != null || combatNPCData != null)
        {
            logInUserData = null;
            combatStat = null;
            combatNPCData = null;
        }

        logInUserData = CombatSpawnManager.Instance.logInUserData;
        combatStat = CombatSpawnManager.Instance.combatStat;
        combatNPCData = CombatSpawnManager.Instance.combatNPCData;

        print($"combatStat 넘어옴 : {combatStat.atk}");
        print($"combatStat 넘어옴 : {combatStat.plusAtk}");

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
                GameObject leftCombatUnitObj = Instantiate(race, leftSpawnPos.position, Quaternion.identity);
                leftCombatUnitObj.transform.rotation = race.transform.localRotation;

                if (leftCombatUnitObj != null)
                {
                    print("1");
                    LeftCombatUnit leftCombatUnit = leftCombatUnitObj.GetComponent<LeftCombatUnit>();

                    print("2");
                    //LeftCombatUnit newUnit = new LeftCombatUnit(logInUserData, combatStat);
                    leftCombatUnit.nickName = logInUserData.nickname;
                    leftCombatUnit.atk = combatStat.atk + combatStat.plusAtk;
                    leftCombatUnit.def = combatStat.def + combatStat.plusDef;
                    leftCombatUnit.hp = combatStat.hp + combatStat.plusHp;
                    leftCombatUnit.luck = combatStat.luck + combatStat.plusLuck;

                    print("3");
                    this.leftCombatUnit = leftCombatUnit;

                    print($"왼쪽 유닛 지정 완료 : {this.leftCombatUnit.name}");
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

    public string lobbyName = "HeeLobbyTest";
    public void GoLobby()
    {
        SceneManager.LoadScene( lobbyName );
    }
}
