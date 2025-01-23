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
                    0.13f,
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

        // �� ���� ũ�� ���
        float cellWidth = width / gridSize;
        float cellHeight = height / gridSize;

        Vector3 cageCenter = transform.position;

        // ���(���� Y = +15) ��ǥ ���
        for (int col = 0; col < gridSize; col++)
        {
            float x = cageCenter.x - width / 2f + cellWidth * (col + 0.5f);
            float y = 0.13f;
            float z = cageCenter.z + height / 2f; // ���� Y = +15
            _outerCellCenters.Add(new Vector3(x, y, z));
            Vector3 cellCenter = new Vector3(x, y, z);
            Instantiate(redSphere, cellCenter, Quaternion.identity);
        }

        // �ϴ�(�Ʒ��� Y = -15) ��ǥ ���
        for (int col = 0; col < gridSize; col++)
        {
            float x = cageCenter.x - width / 2f + cellWidth * (col + 0.5f);
            float y = 0.13f;
            float z = cageCenter.z - height / 2f; // �Ʒ��� Y = -15
            _outerCellCenters.Add(new Vector3(x, y, z));
            Vector3 cellCenter = new Vector3(x, y, z);
            Instantiate(redSphere, cellCenter, Quaternion.identity);
        }

        // ����(X = -15) ��ǥ ���
        for (int row = 0; row < gridSize; row++)
        {
            float x = cageCenter.x - width / 2f; // ���� X = -15
            float y = 0.13f;
            float z = cageCenter.z - height / 2f + cellHeight * (row + 0.5f);
            _outerCellCenters.Add(new Vector3(x, y, z));
            Vector3 cellCenter = new Vector3(x, y, z);
            Instantiate(redSphere, cellCenter, Quaternion.identity);
        }

        // ������(X = +15) ��ǥ ���
        for (int row = 0; row < gridSize; row++)
        {
            float x = cageCenter.x + width / 2f; // ������ X = +15
            float y = 0.13f;
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

