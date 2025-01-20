using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[System.Serializable]
public enum RoomState
{
    Waiting,
    Playing,
    Finished
}

[System.Serializable]
public class RoomData
{
    public string roomKey;
    public string roomName;
    public string serverName;
    public int betting;
    public string host;
    public string guest;
    public bool isHostTurn;
    public int turnCount;
    public bool hostExited;
    public bool guestExited;
    public RoomState state;
    public List<Turn> turnList;

    public RoomData() { }

    //방 만들때 필요한 생성자
    public RoomData(string roomName, int betting)
    {
        this.roomKey = "";
        this.roomName = roomName;
        this.betting = betting;
        this.serverName = "";
        this.host = "";
        this.guest = "";
        isHostTurn = true;
        turnCount = 0;
        hostExited = false;
        guestExited = false;
        state = RoomState.Waiting;
        turnList = new List<Turn>();
    }

    public RoomData(string roomKey, string roomName, int betting, string serverName, string host)
    {
        this.roomKey = roomKey;
        this.roomName = roomName;
        this.betting = betting;
        this.serverName = serverName;
        this.host = host;
        this.guest = "";
        isHostTurn = true;
        turnCount = 0;
        hostExited = false;
        guestExited = false;
        state = RoomState.Waiting;
        turnList = new List<Turn>();
    }
}

[System.Serializable]
public class Turn
{
    public string coodinate;
    public bool isHostTurn;
    public int turnCount;

    public Turn() { }

    public Turn(string coodinate, bool isHostTurn, int turnCount)
    {
        this.coodinate=coodinate;
        this.isHostTurn=isHostTurn;
        this.turnCount=turnCount;
    }
}
