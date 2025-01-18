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

		skillDone.Clear();

		if (RGLightManager.player.PlayerDistanceTracker.PlayerDistance <= 50f) StartCoroutine(Phase1(count, interval));
		else if (RGLightManager.player.PlayerDistanceTracker.PlayerDistance <= 100f) StartCoroutine(Phase2(count, interval));
		else StartCoroutine(Phase3(count, interval));
	}

	public List<bool> skillDone = new List<bool>();

	public IEnumerator Phase1(int count, float interval)
	{
		for (int i = 0; i < count; i++)
		{
			Skill selectedSkill = _skills[1];

			selectedSkill.UseSkill();

			yield return new WaitForSeconds(interval);
		}

		while (skillDone.Count != count)
		{
			yield return null;
		}

		print("1페이지입니다");
		endSkillAction?.Invoke();
	}

	public IEnumerator Phase2(int count, float interval)
	{
		for (int i = 0; i < count; i++)
		{
			Skill selectedSkill = _skills[0];

			selectedSkill.UseSkill();

			yield return new WaitForSeconds(interval);
		}

		while (skillDone.Count != count)
		{
			yield return null;
		}

		print("2페이지입니다");
		endSkillAction?.Invoke();
	}

	public IEnumerator Phase3(int count, float interval)
	{
		for (int i = 0; i < count; i++)
		{
			Skill selectedSkill = _skills[0];

			selectedSkill.UseSkill();

			yield return new WaitForSeconds(interval);
		}

		while (skillDone.Count != count)
		{
			yield return null;
		}

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
