using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public float width = 30f;
    public float height = 30f;
    public int gridSize = 3;

    private List<Vector3> _cellCenters;

    private void Awake()
    {
        CalculateCellCenters();
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
            }
        }
    }

    public List<Vector3> GetCellCenters()
    {
        return _cellCenters;
    }
}

