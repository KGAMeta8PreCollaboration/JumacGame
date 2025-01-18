using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
	public float width = 30f;
	public float height = 30f;
	public int gridSize = 3;
	public GameObject blueSphere;
	public GameObject redSphere;

	private List<Vector3> _cellCenters;
	private List<Vector3> _outerCellCenters;

	private void Awake()
	{
		CalculateCellCenters();
		CalculateOuterCellCenters();
	}

	private void CalculateCellCenters()
	{
		_cellCenters = new List<Vector3>();
		float cellWidth = width / gridSize;
		float cellHeight = height / gridSize;

		Vector3 cageCenter = transform.position;

		for (int row = 0; row < gridSize; row++)
		{
			for (int col = 0; col < gridSize; col++)
			{
				Vector3 cellCenter = new Vector3(
					cageCenter.x - width / 2f + cellWidth * (col + 0.5f),
					cageCenter.y,
					cageCenter.z - height / 2f + cellHeight * (row + 0.5f)
				);
				_cellCenters.Add(cellCenter);
				Instantiate(blueSphere, cellCenter, Quaternion.identity);
			}
		}
	}

	private void CalculateOuterCellCenters()
	{
		_outerCellCenters = new List<Vector3>();

		// 각 셀의 크기 계산
		float cellWidth = width / gridSize;
		float cellHeight = height / gridSize;

		Vector3 cageCenter = transform.position;

		// 상단(위쪽 Y = +15) 좌표 계산
		for (int col = 0; col < gridSize; col++)
		{
			float x = cageCenter.x - width / 2f + cellWidth * (col + 0.5f);
			float y = cageCenter.y;
			float z = cageCenter.z + height / 2f; // 위쪽 Y = +15
			_outerCellCenters.Add(new Vector3(x, y, z));
			Vector3 cellCenter = new Vector3(x, y, z);
			Instantiate(redSphere, cellCenter, Quaternion.identity);
		}

		// 하단(아래쪽 Y = -15) 좌표 계산
		for (int col = 0; col < gridSize; col++)
		{
			float x = cageCenter.x - width / 2f + cellWidth * (col + 0.5f);
			float y = cageCenter.y;
			float z = cageCenter.z - height / 2f; // 아래쪽 Y = -15
			_outerCellCenters.Add(new Vector3(x, y, z));
			Vector3 cellCenter = new Vector3(x, y, z);
			Instantiate(redSphere, cellCenter, Quaternion.identity);
		}

		// 왼쪽(X = -15) 좌표 계산
		for (int row = 0; row < gridSize; row++)
		{
			float x = cageCenter.x - width / 2f; // 왼쪽 X = -15
			float y = cageCenter.y;
			float z = cageCenter.z - height / 2f + cellHeight * (row + 0.5f);
			_outerCellCenters.Add(new Vector3(x, y, z));
			Vector3 cellCenter = new Vector3(x, y, z);
			Instantiate(redSphere, cellCenter, Quaternion.identity);
		}

		// 오른쪽(X = +15) 좌표 계산
		for (int row = 0; row < gridSize; row++)
		{
			float x = cageCenter.x + width / 2f; // 오른쪽 X = +15
			float y = cageCenter.y;
			float z = cageCenter.z - height / 2f + cellHeight * (row + 0.5f);
			_outerCellCenters.Add(new Vector3(x, y, z));
			Vector3 cellCenter = new Vector3(x, y, z);
			Instantiate(redSphere, cellCenter, Quaternion.identity);
		}
	}

	public List<Vector3> GetCellCenters()
	{
		return _cellCenters;
	}

	public List<Vector3> GetOuterCellCenters()
	{
		return _outerCellCenters;
	}
}

