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
    public string host;
    public string guest;
    public bool isHostTurn;
    public int turnCount;
    public RoomState state;
    public List<Turn> turnList = new List<Turn>();

    public RoomData() { }

    public RoomData(string roomName)
    {
        this.roomKey = "";
        this.roomName = roomName;
        this.serverName = "";
        this.host = "";
        this.guest = "";
        isHostTurn = true;
        turnCount = 0;
        state = RoomState.Waiting;
        turnList = new List<Turn>();
    }

    public RoomData(string roomKey, string roomName, string serverName, string host)
    {
        this.roomKey = roomKey;
        this.roomName = roomName;
        this.serverName = serverName;
        this.host = host;
        this.guest = "";
        isHostTurn = true;
        turnCount = 0;
        state = RoomState.Waiting;
        turnList = new List<Turn>();
    }
}

[System.Serializable]
public class Turn
{
    public bool isHostTurn;
    public string coodinate;
}
