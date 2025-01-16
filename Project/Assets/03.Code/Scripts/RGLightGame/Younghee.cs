using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Younghee : MonoBehaviour
{
    public Action endSkillAction;

    public IEnumerator UseSkill()
    {
        yield return new WaitForSeconds(2f);
        endSkillAction?.Invoke();
    }
}
