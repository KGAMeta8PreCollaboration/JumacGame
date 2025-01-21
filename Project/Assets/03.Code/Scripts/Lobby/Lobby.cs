using System.Collections.Generic;
using UnityEngine;

public partial class Lobby
{
    public string lobbyName;
    public string lobbyOwner;
    public List<User> userList = new List<User>();
    public Dictionary<string, User> userDict = new Dictionary<string, User>();

    public Lobby()
    {
        lobbyName = "lobbyName";
        lobbyOwner = "roomOwner";
    }

    public Lobby(string lobbyName, string lobbyOwner)
    {
        this.lobbyName = lobbyName;
        this.lobbyOwner = lobbyOwner;
        userList.Add(new User(lobbyName));
    }

    public void AddUser(string username, Vector3 position)
    {
        userList.Add(new User(username, position));
    }
}


public partial class Lobby
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

