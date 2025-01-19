using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Raser : Skill
{
	[Header("Settings")]
	public float circleSrcTime = 1f;
	public float circleSrcMax = 1f;

	[Header("Prefabs")]
	[SerializeField] private GameObject _circleIndicateDesPrefab;
	[SerializeField] private GameObject _circleIndicateSrcPrefab;
	[SerializeField] private GameObject _lightningPrefab;

	private Younghee _younghee;

	public override void Init(Younghee younghee)
	{
		_younghee = younghee;
	}

	public override void UseSkill()
	{
		List<Vector3> cellCenters = _younghee.RGLightManager.CageManager.cage.GetCellCenters();

		StartCoroutine(SpawnLightningInCage(cellCenters));
	}

	private IEnumerator SpawnLightningInCage(List<Vector3> cellCenters)
	{
		Vector3 selectedCell = SelectRandomCell(cellCenters);

		bool skillComplete = false;

		StartCoroutine(StrikeLightning(selectedCell, () =>
		{
			skillComplete = true;
		}));

		// 이 코루틴의 skillComplete가 true가 될 때까지 대기
		yield return new WaitUntil(() => skillComplete == true);

		_younghee.skillDone.Add(true);
	}

	private IEnumerator StrikeLightning(Vector3 target, Action onComplete)
	{
		Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
		GameObject circleDes = Instantiate(_circleIndicateDesPrefab, target, rotation);
		GameObject circle = Instantiate(_circleIndicateSrcPrefab, target, rotation);

		yield return StartCoroutine(ExpandCircle(circle));

		GameObject raser = Instantiate(_lightningPrefab, target, Quaternion.identity);

		Destroy(circle);
		Destroy(circleDes);

		yield return new WaitForSeconds(1.5f);
		print("레이저 이펙트 삭제");
		Destroy(raser);

		// 완료 상태를 알림
		onComplete?.Invoke();
	}

	private Vector3 SelectRandomCell(List<Vector3> cellCenters)
	{
		Vector3 selectedCell = cellCenters[Random.Range(0, cellCenters.Count)];

		return selectedCell;
	}

	private IEnumerator ExpandCircle(GameObject circle)
	{
		float startTime = 0f;
		Vector3 startSize = Vector3.zero;
		Vector3 maxSize = new Vector3(circleSrcMax, circleSrcMax, circleSrcMax);

		while (startTime < circleSrcTime)
		{
			startTime += Time.deltaTime;

			float progress = Mathf.Clamp01(startTime / circleSrcTime);

			circle.transform.localScale = Vector3.Lerp(startSize, maxSize, progress);
			yield return null;
		}

		circle.transform.localScale = maxSize;
	}
}
