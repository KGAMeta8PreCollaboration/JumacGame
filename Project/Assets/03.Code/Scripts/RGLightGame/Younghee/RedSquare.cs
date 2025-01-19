using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSquare : Skill
{
	[Header("Settings")]
	public float squareGrowTime = 1f; // 네모가 길어지는 시간
	public float squareMaxHeight = 5.7f; // 네모가 길어질 최대 높이
	public float squareWidth = 1f; // 네모의 가로 크기

	[Header("Prefabs")]
	[SerializeField] private GameObject _squarePrefab; // 빨간 네모 프리팹
	[SerializeField] private GameObject _effectPrefab;

	private Younghee _younghee;

	public override void Init(Younghee younghee)
	{
		_younghee = younghee;
	}

	public override void UseSkill()
	{
		// 케이지의 외곽 점 가져오기
		List<Vector3> outerCellCenters = _younghee.RGLightManager.CageManager.cage.GetOuterCellCenters();

		// 위쪽 점만 필터링
		float topZ = _younghee.RGLightManager.CageManager.cage.transform.position.z +
					 _younghee.RGLightManager.CageManager.cage.height / 2f;
		List<Vector3> topPoints = outerCellCenters.FindAll(point => Mathf.Approximately(point.z, topZ));

		// 위쪽 점에서 1, 3, 5번째 선택
		List<Vector3> oddPoints = new List<Vector3>
		{
			topPoints[0], // 1번째 점
            topPoints[2], // 3번째 점
            topPoints[4]  // 5번째 점
        };
		List<Vector3> evenPoints = new List<Vector3>
		{
			topPoints[1],
			topPoints[3]
		};

		List<Vector3> selectedPoints;

		float random = UnityEngine.Random.Range(0, 100f);
		if (random <= 50) selectedPoints = oddPoints;
		else selectedPoints = evenPoints;

		StartCoroutine(ExecuteSkillAtPoints(selectedPoints));
	}

	private IEnumerator ExecuteSkillAtPoints(List<Vector3> points)
	{
		bool skillComplete = false;

		foreach (var point in points)
		{
			StartCoroutine(StrikeSquare(point, () =>
			{
				skillComplete = true;
			}));
		}

		// 네모가 끝날 때까지 대기
		yield return new WaitUntil(() => skillComplete == true);
		print("RapidFire의 스킬이 끝남");
		_younghee.skillDone.Add(true);
	}

	private IEnumerator StrikeSquare(Vector3 target, Action onComplete)
	{
		Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

		// 빨간 네모 생성
		GameObject square = Instantiate(_squarePrefab, target, rotation);

		// 네모 확장 애니메이션 실행
		yield return StartCoroutine(ExpandSquare(square, squareWidth, squareMaxHeight, squareGrowTime));

		// 이펙트 생성
		GameObject effect = Instantiate(_effectPrefab, target, Quaternion.Euler(-90f, 0f, 0f));

		Destroy(square);

		yield return new WaitForSeconds(1.5f);
		print("기관총 이펙트 삭제");
		Destroy(effect);

		// 완료 상태 알림
		onComplete?.Invoke();
	}

	private IEnumerator ExpandSquare(GameObject square, float squareWidth, float squareMaxHeight, float squareGrowTime)
	{
		// 초기 크기와 최종 크기 설정
		square.transform.localScale = new Vector3(squareWidth, 0f, 1f);
		Vector3 maxSize = new Vector3(squareWidth, squareMaxHeight, 1f);

		float elapsedTime = 0f;

		// 확장 애니메이션
		while (elapsedTime < squareGrowTime)
		{
			elapsedTime += Time.deltaTime;

			float progress = Mathf.Clamp01(elapsedTime / squareGrowTime);
			float newHeight = Mathf.Lerp(0f, squareMaxHeight, progress);
			square.transform.localScale = new Vector3(squareWidth, newHeight, 1f);

			yield return null;
		}

		// 최종 크기 설정
		square.transform.localScale = maxSize;

		yield return new WaitForSeconds(2f);
	}

	private Vector3 SelectRandomCell(List<Vector3> cellCenters)
	{
		Vector3 selectedCell = cellCenters[UnityEngine.Random.Range(0, cellCenters.Count)];
		return selectedCell;
	}
}

