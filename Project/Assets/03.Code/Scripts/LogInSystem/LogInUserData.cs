[System.Serializable]
public class LogInUserData
{
    public string id;
    public string timestamp;
    public string nickname;
    public string race;
    public string serverName;
    public int gold;
    public bool setNicknameRace;

    public LogInUserData(string id, string timestamp = default, string nickname = default, string race = default, string serverName = default, int gold = 0, bool setNicknameRace = false)
    {
        this.id = id;
        this.timestamp = timestamp;
        this.nickname = nickname;
        this.race = race;
        this.serverName = serverName;
        this.gold = gold;
        this.setNicknameRace = setNicknameRace;
    }

    public LogInUserData() { }
}
