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

    private int[,] boardState; //0 -> 기본, 1 -> 흑, 2 -> 백

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

        if (OmokUIManager.Instance.openPopupStack.Count >= 1)
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
                if (IsStoneHere(boardIndex) == true)
                {
                    Debug.LogError("이미 돌이 있음!");
                    return;
                }

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

    private bool IsStoneHere(Vector2Int index)
    {
        if (boardState[index.x, index.y] != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlaceStone(bool isHostTurn, Vector2Int index)
    {
        boardState[index.x, index.y] = isHostTurn ? 1 : 2;
        int isFive = CheckFive(boardState[index.x, index.y]);


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

        if (isFive == 1)
        {
            Debug.LogWarning("오목이 됨");

            ResultPopup(isHostTurn);
        }
    }

    //0 -> None, 1 -> 오목, 2 -> 오목이상
    private int CheckFive(int now)
    {
        for (int i = 0; i < gridCount; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                if (boardState[i, j] != now) continue;

                // ->
                if (IsInRange(j + 4)  
                    && boardState[i, j + 1] == now
                    && boardState[i, j + 2] == now
                    && boardState[i, j + 3] == now
                    && boardState[i, j + 4] == now)
                {
                    return (IsInRange(j + 5) && boardState[i, j + 5] == now) ? 2 : 1;
                }

                // ↓
                else if (IsInRange(i + 4)
                    && boardState[i + 1, j] == now
                    && boardState[i + 2, j] == now
                    && boardState[i + 3, j] == now
                    && boardState[i + 4, j] == now)
                {
                    return (IsInRange(i + 5) && boardState[i + 5, j] == now) ? 2 : 1;
                }

                // ↗
                else if (IsInRange(i + 4, j + 4)
                    && boardState[i + 1, j + 1] == now
                    && boardState[i + 2, j + 2] == now
                    && boardState[i + 3, j + 3] == now
                    && boardState[i + 4, j + 4] == now)
                {
                    return (IsInRange(i + 5, j + 5) && boardState[i + 5, j + 5] == now) ? 2 : 1;
                }

                // ↖
                else if (IsInRange(i - 4, j + 4)
                    && boardState[i - 1, j + 1] == now
                    && boardState[i - 2, j + 2] == now
                    && boardState[i - 3, j + 3] == now
                    && boardState[i - 4, j + 4] == now)
                {
                    return (IsInRange(i - 5, j + 5) && boardState[i - 5, j + 5] == now) ? 2 : 1;
                }
            }
        }
        return 0 ;
    }

    //판을 벗어나지는 않았는지 확인하는 함수
    private bool IsInRange(params int[] v)
    {
        for (int i = 0; i < v.Length; i++)
        {
            if (!(v[i] >= 0 && v[i] < gridCount)) return false; 
        }
        return true;
    }

    private void ResultPopup(bool isHostTurn)
    {
        //나는 호스트인가?
        bool amIHost = OmokFirebaseManager.Instance.AmIHost();

        //승자가 호스트인가?
        bool winnerIsHost = isHostTurn;

        //승자와 내가 상태가 같으면 우승자
        bool iAmWinner = (amIHost == winnerIsHost);

        GameEndUIPage popup = OmokUIManager.Instance.PopupOpen<GameEndUIPage>();
        popup.AmIWinner(iAmWinner);
        OmokFirebaseManager.Instance.UpdateOmokUserData(iAmWinner);
    }
}
