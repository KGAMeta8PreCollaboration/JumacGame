using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Raser : Skill
{
	[Header("Settings")]
	public float circleSrcTime = 1f; // 빨간 원이 채워지는 시간
	public float circleSrcMax = 1f;  // 빨간 원의 최대 크기
	public float circleSpawnInterval;
	public int gridSize = 3;         // 케이지의 분할 크기 (예: 3x3)
	public float cageWidth = 30f;    // 케이지 가로 길이
	public float cageHeight = 30f;   // 케이지 세로 길이
	public int strikeCount = 3;      // 번개 타격 개수

	[Header("Prefabs")]
	[SerializeField] private GameObject _circleIndicateDesPrefab;
	[SerializeField] private GameObject _circleIndicateSrcPrefab;
	[SerializeField] private GameObject _lightningPrefab;

	private Younghee _younghee;

	public override void Init(Younghee younghee)
	{
		_younghee = younghee;
	}

	// 스킬 사용 시 호출
	public override void UseSkill()
	{
		Vector3 cageCenter = _younghee.RGLightManager.player.PlayerRay.CalcSpawnPoint();
		StartCoroutine(SpawnLightningInCage(cageCenter));
	}

	// 번개를 케이지 내에서 랜덤으로 떨어뜨리는 코루틴
	private IEnumerator SpawnLightningInCage(Vector3 cageCenter)
	{
		// 케이지 분할 및 랜덤 셀 선택
		List<Vector3> cellCenters = CalculateCellCenters(cageCenter, cageWidth, cageHeight, gridSize);
		List<Vector3> selectedCells = SelectRandomCells(cellCenters, strikeCount);

		// 선택된 칸에 번개 타격 실행
		foreach (Vector3 target in selectedCells)
		{

			StartCoroutine(StrikeLightning(target));
			yield return new WaitForSeconds(circleSpawnInterval);
		}

		yield return new WaitForSeconds(circleSrcTime + 0.1f);

		CompleteSkill();
	}

	// 번개 타격 처리 (빨간 원 생성 및 번개 생성)
	private IEnumerator StrikeLightning(Vector3 target)
	{
		// 빨간 원 생성
		Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
		GameObject circleDes = Instantiate(_circleIndicateDesPrefab, target, rotation);
		GameObject circle = Instantiate(_circleIndicateSrcPrefab, target, rotation);

		// 빨간 원 확장 애니메이션 실행
		yield return StartCoroutine(ExpandCircle(circle));
		// 번개 생성
		Instantiate(_lightningPrefab, target, Quaternion.identity);

		// 빨간 원 삭제
		Destroy(circle);
		Destroy(circleDes);
	}

	// 케이지를 분할하고 각 셀의 중심점을 계산
	private List<Vector3> CalculateCellCenters(Vector3 center, float width, float height, int gridSize)
	{
		List<Vector3> cellCenters = new List<Vector3>();
		float cellWidth = width / gridSize;
		float cellHeight = height / gridSize;

		for (int row = 0; row < gridSize; row++)
		{
			for (int col = 0; col < gridSize; col++)
			{
				Vector3 cellCenter = new Vector3(
					center.x - width / 2f + cellWidth * (col + 0.5f),
					0.14f,
					center.z - height / 2f + cellHeight * (row + 0.5f)
				);
				cellCenters.Add(cellCenter);
			}
		}

		return cellCenters;
	}

	// 셀 목록에서 랜덤으로 지정된 개수만큼 선택
	private List<Vector3> SelectRandomCells(List<Vector3> cellCenters, int count)
	{
		List<Vector3> selectedCells = new List<Vector3>();

		while (selectedCells.Count < count)
		{
			Vector3 randomCell = cellCenters[Random.Range(0, cellCenters.Count)];
			if (!selectedCells.Contains(randomCell))
			{
				selectedCells.Add(randomCell);
			}
		}

		return selectedCells;
	}

	// 빨간 원이 점점 커지는 애니메이션
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
