using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
	public Action onSkillComplete;

	public abstract void UseSkill();

	public abstract void Init(Younghee younghee);

	protected void CompleteSkill()
	{
		onSkillComplete?.Invoke();
	}
}
