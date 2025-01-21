using System.Collections.Generic;
using UnityEngine;


public partial class LobbyData
{
    public string lobbyName;
    public Dictionary<string, User> userList = new Dictionary<string, User>();

    public LobbyData()
    {
        lobbyName = "lobbyName";
    }

    public LobbyData(string lobbyName, string lobbyOwner)
    {
        this.lobbyName = lobbyName;
    }

    public void AddUser(string uid, string username, Vector3 position)
    {
        userList.Add(uid, new User(username, position));
    }
}


public partial class LobbyData
{
    /// <summary>
    /// Lobby.User
    /// </summary>
    public class User
    {
        public string username;
        public Point position;

        public User()
        {
            username = "username";
            position = new Point(0,0,0);
        }
        public User(string username)
        {
            this.username = username;
            position = new Point(0,0,0);
        }
        public User(string username, Vector3 pos)
        {
            this.username = username;
            position = new Point(pos.x,pos.y,pos.z);
        }
    }
}

