using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public abstract void UseSkill();

    public abstract void Init(Younghee younghee);
}
