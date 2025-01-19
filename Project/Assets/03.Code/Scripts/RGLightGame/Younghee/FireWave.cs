using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireWave : Skill
{
	[Header("Settings")]
	public float squareGrowTime = 1f;
	public float squareMaxHeight = 5.7f;
	public float squareWidth = 1f;
	public float skillInterval = 0.5f;

	[Header("Prefabs")]
	[SerializeField] private GameObject _squarePrefab;
	[SerializeField] private GameObject _effectPrefab;

	private Younghee _younghee;

	public override void Init(Younghee younghee)
	{
		_younghee = younghee;
	}

	public override void UseSkill()
	{
		// 모든 셀 좌표 가져오기
		List<Vector3> allCellCenters = _younghee.RGLightManager.CageManager.cage.GetCellCenters();

		// 외곽 점 가져오기
		List<Vector3> outerCellCenters = _younghee.RGLightManager.CageManager.cage.GetOuterCellCenters();

		// 왼쪽 외곽 점 필터링
		float leftX = _younghee.RGLightManager.CageManager.cage.transform.position.x -
					  _younghee.RGLightManager.CageManager.cage.width / 2f;
		List<Vector3> leftPoints = outerCellCenters.FindAll(point => Mathf.Approximately(point.x, leftX));

		// 왼쪽 점 중 랜덤으로 하나 선택
		Vector3 startPoint = leftPoints[UnityEngine.Random.Range(0, leftPoints.Count)];

		// 내부 셀 중 동일한 Z 좌표를 가진 점들 필터링 및 정렬
		List<Vector3> wavePath = allCellCenters.FindAll(point =>
			Mathf.Approximately(point.z, startPoint.z)); // 같은 Z 좌표인 점
		wavePath.Sort((a, b) => a.x.CompareTo(b.x)); // X 좌표 기준 정렬

		// 스킬 실행
		StartCoroutine(ExecuteSkillInWave(startPoint, wavePath));
	}

	private IEnumerator ExecuteSkillInWave(Vector3 startPoint, List<Vector3> wavePath)
	{
		// 공격 예고용 빨간 네모 생성
		Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
		GameObject square = Instantiate(_squarePrefab, startPoint, rotation);

		// 빨간 네모 확장
		yield return StartCoroutine(ExpandSquare(square, squareWidth, squareMaxHeight, squareGrowTime));

		Destroy(square);

		List<GameObject> curEffects = new List<GameObject>();

		// 순차적으로 이펙트 생성
		foreach (Vector3 point in wavePath)
		{
			GameObject effect = Instantiate(_effectPrefab, point, Quaternion.identity);
			curEffects.Add(effect);
			yield return new WaitForSeconds(skillInterval); // 점 간 간격 대기
		}

		yield return new WaitForSeconds(3f);

		foreach (GameObject effect in curEffects)
		{
			Destroy(effect);
		}

		// 스킬 완료 처리
		print("FireWave 스킬 완료");
		_younghee.skillDone.Add(true);
	}

	private IEnumerator ExpandSquare(GameObject square, float squareWidth, float squareMaxHeight, float squareGrowTime)
	{
		square.transform.localScale = new Vector3(squareWidth, 0f, 1f);
		Vector3 maxSize = new Vector3(squareMaxHeight, squareWidth, 1f);

		float elapsedTime = 0f;

		while (elapsedTime < squareGrowTime)
		{
			elapsedTime += Time.deltaTime;

			float progress = Mathf.Clamp01(elapsedTime / squareGrowTime);
			float newHeight = Mathf.Lerp(0f, squareMaxHeight, progress);
			square.transform.localScale = new Vector3(newHeight, squareWidth, 1f);

			yield return null;
		}

		square.transform.localScale = maxSize;

		yield return new WaitForSeconds(1f);
	}
}
