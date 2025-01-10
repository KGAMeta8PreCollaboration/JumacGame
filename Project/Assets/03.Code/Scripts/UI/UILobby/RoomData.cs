using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string host;
    public string guest;
    public RoomState state;
    public List<Turn> turnList = new List<Turn>();

    public RoomData() { }

    public RoomData(string roomName)
    {
        this.roomKey = "";
        this.roomName = roomName;
        this.host = "";
        state = RoomState.Waiting;
        turnList = new List<Turn>();
    }

    public RoomData(string roomKey, string roomName, string host)
    {
        this.roomKey = roomKey;
        this.roomName = roomName;
        this.host = host;
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
