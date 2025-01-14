using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject blackStonePrefab;
    [SerializeField] private GameObject whiteStonePrefab;
    [SerializeField] private int gridCount = 15;

    private float boardSize;
    private float cellSize;
    private Vector3 boardStartPos;

    private int[,] boardState;

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        boardSize = renderer.bounds.size.x; // -> 0.47(한 변의 길이)

        cellSize = boardSize / (gridCount - 1); // -> 0.03357143(cell한칸의 크기)
        boardStartPos = new Vector3(-boardSize / 2, 0, -boardSize / 2); //(보드의 시작점 0,0)

        //나중에 OnEable도 생각해봐야겠다.
        boardState = new int[gridCount, gridCount];

        for (int i = 0; i < gridCount; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                boardState[i, j] = 0;
            }
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Vector2 inputPosition = GetInputPosition();

        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //여기에서 실제 포인트를 index로 치환
            Vector2Int boardIndex = GetBoardIndex(hit.point);

            if (boardIndex.x >= 0 && boardIndex.x < gridCount && 
                boardIndex.y >= 0 && boardIndex.y < gridCount)
            {
                print(boardIndex);
                OmokFirebaseManager.Instance.RequestPlaceStone(boardIndex);
            }
            else
            {
                Debug.LogWarning("거긴 보드가 아님!");
            }
        }
    }

    private Vector2 GetInputPosition()
    {
        if (InputSystem.GetDevice<Touchscreen>() != null)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();

        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            return Mouse.current.position.ReadValue();
        }
        return Vector2.zero;
    }

    private Vector2Int GetBoardIndex(Vector3 position)
    {
        float xOffset = position.x - boardStartPos.x;
        float zOffset = position.z - boardStartPos.z;

        int col = Mathf.RoundToInt(xOffset / cellSize);
        int row = Mathf.RoundToInt(zOffset / cellSize);

        return new Vector2Int(col, row);
    }

    public void PlaceStone(bool isHostTurn, Vector2Int index)
    {
        if (boardState[index.x, index.y] != 0)
        {
            Debug.LogError($"이미 돌이 있습니다!");
            return;
        }

        boardState[index.x, index.y] = isHostTurn ? 1 : 2;

        GameObject stonePrefab = isHostTurn ? blackStonePrefab : whiteStonePrefab;
        GameObject stone = Instantiate(stonePrefab);

        Vector3 position = new Vector3(
            boardStartPos.x + index.x * cellSize,
            0.0928f,
            boardStartPos.z + index.y * cellSize
            );

        stone.transform.position = position;
        //stone.transform.localScale = new Vector3(556, 556, 556);
        stone.transform.SetParent(transform);
    }
}
