using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JegiGameManager : MonoBehaviour
{
    [SerializeField] private JegiTest jegiPrefab;
    private JegiTest jegi;

    private void Start()
    {
        if (jegi == null)
        {
            jegi = Instantiate(jegiPrefab, new Vector2(0, 0), Quaternion.identity);  // 제기 객체 생성
        }

    }

    public void OnClick(InputAction.CallbackContext context)
    {
        //if (jegi == null)
        //{
        //    jegi = Instantiate(jegiPrefab, new Vector2(0, 0), Quaternion.identity);  // 제기 객체 생성
        //}

        if (!context.performed) return;

        if (InputSystem.GetDevice<Touchscreen>() != null || Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            print("클릭됨");
            float timeDifference = Mathf.Abs(Time.time - jegi.transform.position.y);
            string timing = "Normal";

            if (timeDifference <= 0.2f)
            {
                timing = "Perfect";  // 0.2초 이내면 Perfect
            }
            else if (timeDifference <= 0.4f)
            {
                timing = "Great";  // 0.4초 이내면 Great
            }

            jegi.Touch(timing);  // 타이밍에 맞춰 제기차기 시작
                                 //rb.velocity = new Vector2(2, 2);
        }
    }
}
