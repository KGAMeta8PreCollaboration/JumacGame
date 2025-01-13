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
        boardSize = renderer.bounds.size.x; // -> 0.47(보드판 변의 길이)

        cellSize = boardSize / (gridCount - 1); // -> 0.03357143(cell한칸 크기)
        boardStartPos = new Vector3(-boardSize / 2, 0, -boardSize / 2); //(좌측 아래가 0,0)

        //보드판 초기화인데 OnEnable에 들어와야할지도
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

        // Raycast로 월드 좌표 계산
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector2Int boardIndex = GetBoardIndex(hit.point);

            if (boardIndex.x >= 0 && boardIndex.x < gridCount && 
                boardIndex.y >= 0 && boardIndex.y < gridCount)
            {
                print(boardIndex);
                OmokFirebaseManager.Instance.RequestPlaceStone(boardIndex);
            }
            else
            {
                Debug.LogWarning("Clicked outside the board!");
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
        return Vector2.zero; // 입력 없음
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
            Debug.LogError($"바둑돌이 이미 있습니다");
            return;
        }

        boardState[index.x, index.y] = isHostTurn ? 1 : 2;


        //Vector3 position = new Vector3(
        //    boardStartPos.x + index.x * cellSize,
        //    0.101704126f,
        //    boardStartPos.z + index.y * cellSize
        //    );
        GameObject stonePrefab = isHostTurn ? blackStonePrefab : whiteStonePrefab;
        GameObject stone = Instantiate(stonePrefab);
        Vector3 position = new Vector3(
            boardStartPos.x + index.x * cellSize,
            0.0928f,
            boardStartPos.z + index.y * cellSize
            );
        stone.transform.position = position;
        //stone.transform.localScale = new Vector3(556, 556, 556);
        //stone.transform.localPosition =  new Vector3(stone.transform.localPosition.x, 1.0225521f, stone.transform.localPosition.z);
        stone.transform.SetParent(transform);
    }
}
