using System;
using UnityEngine;

[Serializable]
public class Dialogue : MonoBehaviour
{
    public new string name;
    [TextArea(3, 10)]
    public string[] sentences;
}
