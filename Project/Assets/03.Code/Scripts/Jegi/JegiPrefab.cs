using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class JegiPrefab : MonoBehaviour
{
    public Rigidbody2D _rb;
    private bool _isGrounded = false;
    public bool _isKicked = false;

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
        //그냥 위아래로 움직이기
        //float verticalVel = _rb.velocity.y;

        //if (verticalVel > 0.01f)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, 0);
        //}

        ////내려가면 찬 상태가 초기화
        //else if (verticalVel < -0.01f)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, 180);
        //    _isKicked = false;
        //}

        if (JegiGameManager.Instance._isGameOver == true) return;

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

            //angle을 -90해줘야 가는 방향으로 로테이션이 돈다
            angle += 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            //도는 속도
            float rotationSpeed = 5f;

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            //_isKicked = false;
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -180f);
        }

        _lastVelY = currentVelY;
    }

    public void Kick(float force, float angle)
    {
        print("제기 참");
        //디버깅 중이라 땅에 떨어질때 경우 해제
        if (JegiGameManager.Instance._isGameOver == true) return;

        _isKicked = true;
        _rb.velocity = Vector2.zero;

        Vector2 kickDir = Quaternion.Euler(0, 0, angle) * Vector2.up;
        _rb.AddForce(kickDir * force, ForceMode2D.Impulse);
    }

    private void ReflectedByWall()
    {
        float dirX = _rb.velocity.x;
        float dirY = _rb.velocity.y;

        _rb.velocity = new Vector2(dirX, dirY);
    }
}
