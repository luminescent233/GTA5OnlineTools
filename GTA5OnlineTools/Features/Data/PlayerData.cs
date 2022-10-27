namespace GTA5OnlineTools.Features.Data;

public class PlayerData
{
    public long RockstarId { get; set; }
    public string PlayerName { get; set; }
    public PlayerInfo PlayerInfo { get; set; }
}

public class PlayerInfo
{
    public bool Host { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public bool GodMode { get; set; }
    public bool NoRagdoll { get; set; }
    public byte WantedLevel { get; set; }
    public float RunSpeed { get; set; }
    public Vector3 Position { get; set; }
}
