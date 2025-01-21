using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RemotePlayer : MonoBehaviour
{
    public string username;
    public string UID;

    [SerializeField] private float speed = 5f;
    [SerializeField] private List<GameObject> characterList;
    
    private Vector3 _position;
    private Vector3 dir;
    private NavMeshAgent _navMeshAgent;
    private string _race;

    private void Reset()
    {
        for (int i = 0; i < transform.Find("Models").childCount; i++)
            characterList.Add(transform.Find("Models").GetChild(i).gameObject);
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(string uid, string nickname, Vector3 pos, string race)
    {
        UID = uid;
        username = nickname;
        position = pos;
        _race = race;
        if (UserRace.human.ToString() == race)
            characterList[0].SetActive(true);
        else if (UserRace.dokkaebi.ToString() == race)
            characterList[1].SetActive(true);
        else if (UserRace.ghost.ToString() == race)
            characterList[2].SetActive(true);
        else
            characterList[3].SetActive(true);
    }
    
    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            transform.position = _position;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        _navMeshAgent?.SetDestination(pos);
    }
}
