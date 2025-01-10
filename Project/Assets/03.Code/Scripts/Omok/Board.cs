using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Cell[] cellArr;
    public Dictionary<string, Cell> cellDic = new Dictionary<string, Cell>();

    private void Awake()
    {
        cellArr = GetComponentsInChildren<Cell>();
        int cellNum = 0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                cellArr[cellNum].coodinate = $"{y + 1}, {x + 1}";
                cellNum++;
            }
        }

        foreach (Cell cell in cellArr)
        {
            cellDic.Add(cell.coodinate, cell);
        }
    }
}
