using Minigame.RGLight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Younghee : MonoBehaviour
{
    public RGLightManager RGLightManager;
    public Action endSkillAction;
    private List<Skill> _skills = new List<Skill>();

    private void Awake()
    {
        FindSkill();
    }

    public IEnumerator UseSkill()
    {
        int random = UnityEngine.Random.Range(0, _skills.Count);
        _skills[random].UseSkill();
        yield return new WaitForSeconds(4f);
        endSkillAction?.Invoke();
    }

    public void FindSkill()
    {
        Skill[] skills = transform.GetComponentsInChildren<Skill>();
        foreach (Skill skill in skills)
        {
            _skills.Add(skill);
            skill.Init(this);
        }
    }
}
