using GTA5OnlineTools.Features.Core;

namespace GTA5OnlineTools.Features.SDK;

public static class Player
{
    /// <summary>
    /// 玩家无敌模式
    /// </summary>
    public static void GodMode(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_God, 0x01);
        else
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_God, 0x00);
    }

    /// <summary>
    /// 玩家生命值
    /// </summary>
    /// <param name="value"></param>
    public static void Health(float value)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        GTA5Mem.Write(pCPed + Offsets.CPed_Health, value);
    }

    /// <summary>
    /// 玩家最大生命值
    /// </summary>
    /// <param name="value"></param>
    public static void HealthMax(float value)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        GTA5Mem.Write(pCPed + Offsets.CPed_HealthMax, value);
    }

    /// <summary>
    /// 玩家护甲值
    /// </summary>
    /// <param name="value"></param>
    public static void Armor(float value)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        GTA5Mem.Write(pCPed + Offsets.CPed_Armor, value);
    }

    /// <summary>
    /// 玩家通缉等级，0x00,0x01,0x02,0x03,0x04,0x05
    /// </summary>
    public static void WantedLevel(byte level)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCPlayerInfo = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);

        GTA5Mem.Write(pCPlayerInfo + Offsets.CPed_CPlayerInfo_WantedLevel, level);
    }

    /// <summary>
    /// 玩家奔跑速度
    /// </summary>
    /// <param name="value"></param>
    public static void RunSpeed(float value)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCPlayerInfo = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);
        GTA5Mem.Write(pCPlayerInfo + Offsets.CPed_CPlayerInfo_RunSpeed, value);
    }

    /// <summary>
    /// 玩家游泳速度
    /// </summary>
    /// <param name="value"></param>
    public static void SwimSpeed(float value)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCPlayerInfo = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);
        GTA5Mem.Write(pCPlayerInfo + Offsets.CPed_CPlayerInfo_SwimSpeed, value);
    }

    /// <summary>
    /// 玩家行走速度
    /// </summary>
    /// <param name="value"></param>
    public static void WalkSpeed(float value)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCPlayerInfo = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);
        GTA5Mem.Write(pCPlayerInfo + Offsets.CPed_CPlayerInfo_WalkSpeed, value);
    }

    /// <summary>
    /// 挂机防踢
    /// </summary>
    /// <param name="isEnable"></param>
    public static void AntiAFK(bool isEnable)
    {
        /*
         idleKick: {('262145', '87')}    GLOBAL IDLEKICK_WARNING1 
         idleKick: {('262145', '88')}    GLOBAL IDLEKICK_WARNING2 
         idleKick: {('262145', '89')}    GLOBAL IDLEKICK_WARNING3 
         idleKick: {('262145', '90')}    GLOBAL IDLEKICK_KICK 
         idleKick: {('262145', '8248')}    GLOBAL ConstrainedKick_Warning1 
         idleKick: {('262145', '8249')}    GLOBAL ConstrainedKick_Warning2 
         idleKick: {('262145', '8250')}    GLOBAL ConstrainedKick_Warning3 
         idleKick: {('262145', '8251')}    GLOBAL ConstrainedKick_Kick 
         timeStoodIdle: {('1648034', '1156')}    GLOBAL time in ms  
         idleKick: {('1648034', '1172')}    GLOBAL
         */

        // joaat("weapon_minigun");
        Hacks.WriteGA<int>(262145 + 87, isEnable ? 99999999 : 120000);        // 120000     GLOBAL IDLEKICK_WARNING1 
        Hacks.WriteGA<int>(262145 + 88, isEnable ? 99999999 : 300000);        // 300000     GLOBAL IDLEKICK_WARNING2 
        Hacks.WriteGA<int>(262145 + 89, isEnable ? 99999999 : 600000);        // 600000     GLOBAL IDLEKICK_WARNING3
        Hacks.WriteGA<int>(262145 + 90, isEnable ? 99999999 : 900000);        // 900000     GLOBAL IDLEKICK_KICK 
        // 742014
        Hacks.WriteGA<int>(262145 + 8248, isEnable ? 2000000000 : 30000);     // 30000      GLOBAL ConstrainedKick_Warning1
        Hacks.WriteGA<int>(262145 + 8249, isEnable ? 2000000000 : 60000);     // 60000      GLOBAL ConstrainedKick_Warning2
        Hacks.WriteGA<int>(262145 + 8250, isEnable ? 2000000000 : 90000);     // 90000      GLOBAL ConstrainedKick_Warning3
        Hacks.WriteGA<int>(262145 + 8251, isEnable ? 2000000000 : 120000);    // 120000     GLOBAL ConstrainedKick_Kick
    }

    /// <summary>
    /// 无布娃娃
    /// </summary>
    public static void NoRagdoll(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_Ragdoll, 0x01);
        else
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_Ragdoll, 0x20);
    }

    /// <summary>
    /// 无碰撞体积
    /// </summary>
    public static void NoCollision(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCNavigation = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CNavigation);
        long pointer = GTA5Mem.Read<long>(pCNavigation + 0x10);
        pointer = GTA5Mem.Read<long>(pointer + 0x20);
        pointer = GTA5Mem.Read<long>(pointer + 0x70);
        pointer = GTA5Mem.Read<long>(pointer + 0x00);

        if (isEnable)
            GTA5Mem.Write(pointer + 0x2C, -1.0f);
        else
            GTA5Mem.Write(pointer + 0x2C, 0.25f);
    }

    /// <summary>
    /// 角色防子弹
    /// </summary>
    public static void ProofBullet(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 4) : (uint)(oProof & ~(1 << 4));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防火烧
    /// </summary>
    public static void ProofFire(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 5) : (uint)(oProof & ~(1 << 5));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防撞击
    /// </summary>
    public static void ProofCollision(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 6) : (uint)(oProof & ~(1 << 6));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防近战
    /// </summary>
    public static void ProofMelee(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 7) : (uint)(oProof & ~(1 << 7));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防无敌
    /// </summary>
    public static void ProofGod(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 8) : (uint)(oProof & ~(1 << 8));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防爆炸
    /// </summary>
    public static void ProofExplosion(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 11) : (uint)(oProof & ~(1 << 11));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防蒸汽
    /// </summary>
    public static void ProofSteam(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 15) : (uint)(oProof & ~(1 << 15));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防溺水
    /// </summary>
    public static void ProofDrown(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 16) : (uint)(oProof & ~(1 << 16));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色防海水
    /// </summary>
    public static void ProofWater(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        uint oProof = GTA5Mem.Read<uint>(pCPed + Offsets.CPed_Proof);

        oProof = isEnable ? oProof | (1 << 24) : (uint)(oProof & ~(1 << 24));
        GTA5Mem.Write(pCPed + Offsets.CPed_Proof, oProof);
    }

    /// <summary>
    /// 角色隐形（虚假）
    /// </summary>
    public static void Invisible(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_Invisible, 0x01);
        else
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_Invisible, 0x27);
    }

    /// <summary>
    /// 补满血量和护甲
    /// </summary>
    public static void FillHealthArmor()
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        float oHealth = GTA5Mem.Read<float>(pCPed + Offsets.CPed_Health);
        float oHealthMax = GTA5Mem.Read<float>(pCPed + Offsets.CPed_HealthMax);
        if (oHealth < oHealthMax)
        {
            GTA5Mem.Write(pCPed + Offsets.CPed_Health, oHealthMax);
        }
        else
        {
            GTA5Mem.Write(pCPed + Offsets.CPed_Health, 328.0f);
        }

        GTA5Mem.Write(pCPed + Offsets.CPed_Armor, 50.0f);
    }

    /// <summary>
    /// 玩家自杀（设置当前生命值为1.0）
    /// </summary>
    public static void Suicide()
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        GTA5Mem.Write(pCPed + Offsets.CPed_Health, 1.0f);
    }

    /// <summary>
    /// 雷达影踪（最大生命值为0）
    /// </summary>
    public static void UndeadOffRadar(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            GTA5Mem.Write(pCPed + Offsets.CPed_HealthMax, 0.0f);
        else
            GTA5Mem.Write(pCPed + Offsets.CPed_HealthMax, 328.0f);
    }

    /// <summary>
    /// 永不通缉
    /// </summary>
    public static void WantedCanChange(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCPlayerInfo = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);

        if (isEnable)
            GTA5Mem.Write(pCPlayerInfo + Offsets.CPed_CPlayerInfo_WantedCanChange, 1.0f);
        else
            GTA5Mem.Write(pCPlayerInfo + Offsets.CPed_CPlayerInfo_WantedCanChange, 0.0f);
    }
}
