using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTest : MonoBehaviour
{
    public BubblePrefab test;

    private void Start()
    {
        BubblePrefab test2 = Instantiate(test, transform);
        test2.SetText("이건 테스트야");
    }
}
