using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class JegiPrefab : MonoBehaviour
{
    private Rigidbody2D _rb;
    private bool _isGrounded = false;
    public bool _isKicked = false;

    [Header("해당 콤보마다 가속이 됨")]
    [SerializeField] private int accelerateCombo;

    [Header("얼마나 가속할지")]
    [SerializeField] private float accelerateAmount;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            //여기에서 게임오버를 줘야함 아마 GameManager?
            JegiGameManager.Instance.GameOver();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            ReflectedByWall();
        }
    }

    //이전 프레임의 y좌표
    private float _lastVelY;

    private void Update()
    {
        if (JegiGameManager.Instance.isGameOver == true) return;

        Vector2 velocity = _rb.velocity;
        float currentVelY = velocity.y;

        if (_lastVelY > 0f && currentVelY <0f)
        {
            _isKicked = false;
        }

        float speed = velocity.magnitude;

        //이동 중이라면
        if (speed > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            //angle을 +90해줘야 가는 방향으로 머리가 향함
            angle += 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            //도는 속도
            float rotationSpeed = 5f;

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -180f);
        }

        _lastVelY = currentVelY;
    }

    public void Kick(float force, float angle)
    {
        if (JegiGameManager.Instance.isGameOver == true) return;

        _isKicked = true;
        _rb.velocity = Vector2.zero;

        Vector2 kickDir = Quaternion.Euler(0, 0, angle) * Vector2.up;
        _rb.AddForce(kickDir * force, ForceMode2D.Impulse);

        AccelerateGravity(JegiGameManager.Instance.combo);
    }

    private void ReflectedByWall()
    {
        float dirX = -_rb.velocity.x;
        float dirY = _rb.velocity.y;

        _rb.velocity = new Vector2(dirX, dirY);
    }

    private int _currentMul = 0;
    private int _beforeMul = 0;
    private void AccelerateGravity(int combo)
    {
        _currentMul = (combo / accelerateCombo);
        float accelerateValue = _currentMul * accelerateAmount;

        if (_currentMul != _beforeMul)
        {
            _rb.gravityScale = 0.7f + accelerateValue;
            _beforeMul = _currentMul;
        }
    }
}
