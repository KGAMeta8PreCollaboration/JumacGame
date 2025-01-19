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
		// ��� �� ��ǥ ��������
		List<Vector3> allCellCenters = _younghee.RGLightManager.CageManager.cage.GetCellCenters();

		// �ܰ� �� ��������
		List<Vector3> outerCellCenters = _younghee.RGLightManager.CageManager.cage.GetOuterCellCenters();

		// ���� �ܰ� �� ���͸�
		float leftX = _younghee.RGLightManager.CageManager.cage.transform.position.x -
					  _younghee.RGLightManager.CageManager.cage.width / 2f;
		List<Vector3> leftPoints = outerCellCenters.FindAll(point => Mathf.Approximately(point.x, leftX));

		// ���� �� �� �������� �ϳ� ����
		Vector3 startPoint = leftPoints[UnityEngine.Random.Range(0, leftPoints.Count)];

		// ���� �� �� ������ Z ��ǥ�� ���� ���� ���͸� �� ����
		List<Vector3> wavePath = allCellCenters.FindAll(point =>
			Mathf.Approximately(point.z, startPoint.z)); // ���� Z ��ǥ�� ��
		wavePath.Sort((a, b) => a.x.CompareTo(b.x)); // X ��ǥ ���� ����

		// ��ų ����
		StartCoroutine(ExecuteSkillInWave(startPoint, wavePath));
	}

	private IEnumerator ExecuteSkillInWave(Vector3 startPoint, List<Vector3> wavePath)
	{
		// ���� ����� ���� �׸� ����
		Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
		GameObject square = Instantiate(_squarePrefab, startPoint, rotation);

		// ���� �׸� Ȯ��
		yield return StartCoroutine(ExpandSquare(square, squareWidth, squareMaxHeight, squareGrowTime));

		Destroy(square);

		List<GameObject> curEffects = new List<GameObject>();

		// ���������� ����Ʈ ����
		foreach (Vector3 point in wavePath)
		{
			GameObject effect = Instantiate(_effectPrefab, point, Quaternion.identity);
			curEffects.Add(effect);
			yield return new WaitForSeconds(skillInterval); // �� �� ���� ���
		}

		yield return new WaitForSeconds(3f);

		foreach (GameObject effect in curEffects)
		{
			Destroy(effect);
		}

		// ��ų �Ϸ� ó��
		print("FireWave ��ų �Ϸ�");
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
