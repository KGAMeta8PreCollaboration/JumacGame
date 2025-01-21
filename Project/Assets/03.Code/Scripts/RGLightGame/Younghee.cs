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
			Skill selectedSkill;
			float random = UnityEngine.Random.Range(0, 100f);
			if (random <= 90)
			{
				selectedSkill = _skills[0];
				print("레이저 스킬");
			}
			else
			{
				selectedSkill = _skills[2];
				print("파도 스킬");
			}

			selectedSkill.UseSkill();

			yield return new WaitForSeconds(interval);
		}

		while (skillDone.Count != count)
		{
			yield return null;
		}

		print("1페이지 스킬이 모두 종료되었습니다");
		endSkillAction?.Invoke();
	}

	public IEnumerator Phase2(int count, float interval)
	{
		for (int i = 0; i < count; i++)
		{
			Skill selectedSkill;
			float random = UnityEngine.Random.Range(0, 100f);
			if (random <= 60)
			{
				selectedSkill = _skills[0];
				print("레이저 스킬");
			}
			else if (random <= 90)
			{
				selectedSkill = _skills[2];
				print("파도 스킬");
			}
			else
			{
				selectedSkill = _skills[1];
				print("기관총 스킬");
			}

			selectedSkill.UseSkill();

			yield return new WaitForSeconds(interval);
		}

		while (skillDone.Count != count)
		{
			yield return null;
		}

		print("2페이지 스킬이 모두 종료되었습니다");
		endSkillAction?.Invoke();
	}

	public IEnumerator Phase3(int count, float interval)
	{
		for (int i = 0; i < count; i++)
		{
			Skill selectedSkill;
			float random = UnityEngine.Random.Range(0, 100f);
			if (random <= 40)
			{
				selectedSkill = _skills[0];
				print("레이저 스킬");
			}
			else if (random <= 70)
			{
				selectedSkill = _skills[2];
				print("파도 스킬");
			}
			else
			{
				selectedSkill = _skills[1];
				print("기관총 스킬");
			}

			selectedSkill.UseSkill();

			yield return new WaitForSeconds(interval);
		}

		while (skillDone.Count != count)
		{
			yield return null;
		}

		print("3페이지 스킬이 모두 종료되었습니다");
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
