using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JegiTest : MonoBehaviour
{
    [SerializeField] private float perfectForce = 10f;
    [SerializeField] private float greatForce = 7f;
    [SerializeField] private float normalForce = 4f;
    [SerializeField] private float forceDirection = 1f;
    [SerializeField] private Rigidbody2D rb;
    private bool _isFlying = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y <= -4f && !_isFlying)
        {
            // 바닥에 닿으면 제기를 다시 시작할 준비
            rb.velocity = Vector2.zero;
            _isFlying = false;
        }
    }

    public void Touch(string timing)
    {
        if (!_isFlying)
        {
            _isFlying = true;
            rb.gravityScale = 1f;  // 중력 적용
            ApplyForce(timing);    // 타이밍에 맞는 속도 적용
        }
    }

    private void ApplyForce(string timing)
    {
        float forceY = 0f;

        // 타이밍에 따라 다른 속도 설정
        if (timing == "Perfect")
        {
            forceY = perfectForce;
        }
        else if (timing == "Great")
        {
            forceY = greatForce;
        }
        else if (timing == "Normal")
        {
            forceY = normalForce;
        }

        // 위로 튕겨 올라가는 힘을 Rigidbody2D에 적용
        rb.velocity = new Vector2(0, forceY);  // y 방향으로만 힘을 줌
    }
}
