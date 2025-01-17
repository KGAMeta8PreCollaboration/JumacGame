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

    public void UseSkill()
    {
        int count = RGLightManager.player.PlayerDistanceTracker.GetSkillCount();
        float interval = RGLightManager.player.PlayerDistanceTracker.GetSkillInterval();

        if (RGLightManager.player.PlayerDistanceTracker.PlayerDistance <= 50f) StartCoroutine(Phase1(count, interval));
        else if (RGLightManager.player.PlayerDistanceTracker.PlayerDistance <= 100f) StartCoroutine(Phase2(count, interval));
        else StartCoroutine(Phase3(count, interval));
    }

    public IEnumerator Phase1(int count, float interval)
    {
        List<bool> skillComplete = new List<bool>();

        for (int i = 0; i < count; i++)
        {
            Skill selectedSkill = _skills[0];

            skillComplete.Add(false);

            int index = i;
            selectedSkill.onSkillComplete += () => skillComplete[index] = true;

            selectedSkill.UseSkill();

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitUntil(() => skillComplete.TrueForAll(status => status));
        print("1페이지입니다");
        endSkillAction?.Invoke();
    }

    public IEnumerator Phase2(int count, float interval)
    {
        List<bool> skillComplete = new List<bool>();

        for (int i = 0; i < count; i++)
        {
            Skill selectedSkill = _skills[0];

            skillComplete.Add(false);

            int index = i;
            selectedSkill.onSkillComplete += () => skillComplete[index] = true;

            selectedSkill.UseSkill();

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitUntil(() => skillComplete.TrueForAll(status => status));
        print("2페이지입니다");
        endSkillAction?.Invoke();
    }

    public IEnumerator Phase3(int count, float interval)
    {
        List<bool> skillComplete = new List<bool>();

        for (int i = 0; i < count; i++)
        {
            Skill selectedSkill = _skills[0];

            skillComplete.Add(false);

            int index = i;
            selectedSkill.onSkillComplete += () => skillComplete[index] = true;

            selectedSkill.UseSkill();

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitUntil(() => skillComplete.TrueForAll(status => status));
        print("3페이지입니다");
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
