using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempOmok : MonoBehaviour
{
    public Image e;

    private void Start()
    {
        print($"e의 월드좌표 : {e.rectTransform.position}");        
    }
}
