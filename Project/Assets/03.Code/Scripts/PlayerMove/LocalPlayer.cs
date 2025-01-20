using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class LocalPlayer : MonoBehaviour
{
    public string nickname;
    public string UID;
    public string race;
    public float speed = 5f;
    public float jumpPower = 5.0f;
    private Vector3 _position;
    private Vector3 dir;
    public Vector3 moveDirection = Vector3.zero;
    public Rigidbody rb;

    private Stat _stat;
    private bool _isJump;

    [SerializeField] private List<GameObject> characterList;

    private void Reset()
    {
        speed = 10f;
        jumpPower = 40f;
        // 자식 오브젝트의 이름중 Models오브젝트의 자식들을 리스트에 저장
        for (int i = 0; i < transform.Find("Models").childCount; i++)
            characterList.Add(transform.Find("Models").GetChild(i).gameObject);
    }

    public void Init(string uid, string nickname, string race)
    {
        UID = uid;
        this.nickname = nickname;
        this.race = race;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _stat = GetComponent<Stat>();
        _isJump = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void Jump()
    {
        if (_isJump)
            return;
        _isJump = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        // 부딪힌 방향이 바닥인지를 확인하고 바닥이면 점프 가능한 상태로 만들어준다.
        if (other.contacts[0].normal.y > 0.7f)
            _isJump = false;
    }

    private void LateUpdate()
    {
        Vector3 forwardMove = transform.forward * moveDirection.z * speed * Time.deltaTime;
        Vector3 rightMove = transform.right * moveDirection.x * speed * Time.deltaTime;
        Vector3 move = forwardMove + rightMove;
        // print(transform.position - move);
        rb.MovePosition(transform.position - move);
    }
}
