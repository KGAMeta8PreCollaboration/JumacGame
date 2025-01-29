using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject blackStonePrefab;
    [SerializeField] private GameObject whiteStonePrefab;
    [SerializeField] private GameObject redStonePrefab;
    [SerializeField] UnityEngine.UI.Button doPlaceButton;
    [SerializeField] private int gridCount = 15;

    private float _boardSize;
    private float _cellSize;
    private Vector3 _boardStartPos;
    private Vector2Int? tempStoneIndex = null; //임시 돌의 위치(없으면 null)

    private int[,] boardState; //0 -> 기본, 1 -> 흑, 2 -> 백

    //private void OnEnable()
    //{
    //    doPlaceButton.onClick.AddListener(OnClickDoPlaceStoneButton);
    //}

    private void OnDisable()
    {
        doPlaceButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        _boardSize = renderer.bounds.size.x; // -> 0.47(한 변의 길이)

        _cellSize = _boardSize / (gridCount - 1); // -> 0.03357143(cell한칸의 크기)
        _boardStartPos = new Vector3(-_boardSize / 2, 0, -_boardSize / 2); //(보드의 시작점 0,0)

        //나중에 OnEable도 생각해봐야겠다.
        boardState = new int[gridCount, gridCount];

        for (int i = 0; i < gridCount; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                boardState[i, j] = 0;
            }
        }
        doPlaceButton.onClick.AddListener(OnClickDoPlaceStoneButton);
    }


    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (OmokUIManager.Instance.openPopupStack.Count >= 1)
            return;

        //if (OmokFirebaseManager.Instance.currentRoomData == null)
        //{
        //    Debug.Log("아직 Firebase가 연결되지 않음");
        //    return;
        //}
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
                    Debug.LogWarning("이미 돌이 있음!");
                    return;
                }

                bool isMyTurn = OmokFirebaseManager.Instance.IsMyTurn();
                
                //내 턴일때만 임시로 돌을 놓을 수 있다
                if (isMyTurn)
                {
                    tempStoneIndex = boardIndex;
                    ShowTempStoneImage(boardIndex);
                    //OmokFirebaseManager.Instance.RequestPlaceStone(boardIndex);
                    //Instantiate(tempStoneImagePrefab, new Vector3(boardIndex.x, boardIndex.y, 0), Quaternion.identity, tempStoneUI);
                }
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
        float xOffset = position.x - _boardStartPos.x;
        float zOffset = position.z - _boardStartPos.z;

        int col = Mathf.RoundToInt(xOffset / _cellSize);
        int row = Mathf.RoundToInt(zOffset / _cellSize);

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

    private GameObject _redStoneObj;

    private void ShowTempStoneImage(Vector2Int index)
    {
        if (_redStoneObj != null)
        {
            Destroy(_redStoneObj);
        }

        _redStoneObj = Instantiate(redStonePrefab);

        Vector3 position = new Vector3(
            _boardStartPos.x + index.x * _cellSize,
            0.0928f,
            _boardStartPos.z + index.y * _cellSize
            );

        _redStoneObj.transform.position = position;
        _redStoneObj.transform.SetParent(transform);
    }

    private void OnClickDoPlaceStoneButton()
    {
        if (tempStoneIndex == null)
        {
            Debug.LogWarning("착수 위치를 고르세요");
            return;
        }

        Vector2Int index = tempStoneIndex.Value;

        if (boardState[index.x, index.y] != 0)
        {
            Debug.LogWarning("거기는 돌이 있습니다!");
            return;
        }

        OmokFirebaseManager.Instance.RequestPlaceStone(index);

        //두고나면 임시좌표와 임시프리팹 삭제
        tempStoneIndex = null;
        Destroy(_redStoneObj);
    }

    public void PlaceStone(bool isHostTurn, Vector2Int index)
    {
        boardState[index.x, index.y] = isHostTurn ? 1 : 2;
        int isFive = CheckFive(boardState[index.x, index.y]);


        GameObject stonePrefab = isHostTurn ? blackStonePrefab : whiteStonePrefab;
        GameObject stone = Instantiate(stonePrefab);

        Vector3 position = new Vector3(
            _boardStartPos.x + index.x * _cellSize,
            0.0928f,
            _boardStartPos.z + index.y * _cellSize
            );

        stone.transform.position = position;
        //stone.transform.localScale = new Vector3(556, 556, 556);
        stone.transform.SetParent(transform);

        AudioManager.Instance.PlaySfx(Sfx.OmokTak);

        //오목이 되면
        if (isFive == 1)
        {
            bool result = AmIWinner(isHostTurn);
            ResultPopup(result);

            OmokUserData myData = 
                OmokFirebaseManager.Instance.amIHost ? OmokFirebaseManager.Instance.hostData : OmokFirebaseManager.Instance.guestData;
            OmokFirebaseManager.Instance.UpdateUserResult(myData, result);
            LastTimeHandler.Instance.IsOnGame(false);
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
        OmokOneButtonPopup popup = OmokUIManager.Instance.PopupOpen<OmokOneButtonPopup>();
        popup.SetPopup(isHostTurn, OmokFirebaseManager.Instance.currentRoomData.betting);
    }

    //파라미터로 승리할때의 턴을 넣으면 승리했는지 아닌지 나온다
    private bool AmIWinner(bool isHostTurn)
    {
        //내 턴이 무엇인지 확인 true -> Host, false -> guest
        bool amIHost = GameManager.Instance.FirebaseManager.User.UserId == OmokFirebaseManager.Instance.currentRoomData.host;

        //승리할때 턴 주인 = isHostTurn
        bool winnerIsHost = isHostTurn;

        bool amIWinner = (amIHost == winnerIsHost);

        return amIWinner;
    }
}
