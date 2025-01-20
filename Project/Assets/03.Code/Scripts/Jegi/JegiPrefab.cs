using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class JegiPrefab : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private bool _isGrounded = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!_isGrounded)
            {
                _isGrounded = true;

                //여기에서 게임오버를 줘야함 아마 GameManager?
                JegiGameManager.Instance.GameOver();
            }
        }
    }

    public void Kick(float force)
    {
        //if (_isGrounded) return;

        _rb.velocity = Vector2.zero;
        _rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}
