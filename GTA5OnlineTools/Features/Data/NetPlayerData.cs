namespace GTA5OnlineTools.Features.Data;

public class NetPlayerData
{
    public long RockstarId { get; set; }
    public string PlayerName { get; set; }

    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Armor { get; set; }
    public bool GodMode { get; set; }
    public bool NoRagdoll { get; set; }

    public byte WantedLevel { get; set; }
    public float WalkSpeed { get; set; }
    public float RunSpeed { get; set; }
    public float SwimSpeed { get; set; }
    public bool WantedCanChange { get; set; }
    public int NPCIgnore { get; set; }
    public int FrameFlags { get; set; }

    public Vector3 Position { get; set; }

    public string ClanName { get; set; }
    public string ClanTag { get; set; }

    public bool IsSpectating { get; set; }
    public bool IsRockStarDev { get; set; }
    public bool IsRockStarQA { get; set; }
    public bool IsCheater { get; set; }

    public string RelayIP { get; set; }
    public string ExternalIP { get; set; }
    public string InternalIP { get; set; }

}
