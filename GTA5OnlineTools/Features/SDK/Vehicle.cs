using GTA5OnlineTools.Features.Core;

namespace GTA5OnlineTools.Features.SDK;

public static class Vehicle
{
    /// <summary>
    /// 玩家是否在载具中
    /// </summary>
    public static bool IsInVehicle()
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = GTA5Mem.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        return oInVehicle == 0x01;
    }

    /// <summary>
    /// 载具无敌模式
    /// </summary>
    public static void GodMode(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = GTA5Mem.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CVehicle);
            if (isEnable)
                GTA5Mem.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_God, 0x01);
            else
                GTA5Mem.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_God, 0x00);
        }
    }

    /// <summary>
    /// 载具安全带
    /// </summary>
    public static void Seatbelt(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_Seatbelt, 0xC9);
        else
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_Seatbelt, 0xC8);
    }

    /// <summary>
    /// 载具隐形
    /// </summary>
    public static void Invisible(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_CVehicle_Invisible, 0x01);
        else
            GTA5Mem.Write<byte>(pCPed + Offsets.CPed_CVehicle_Invisible, 0x27);
    }

    /// <summary>
    /// 载具附加功能，默认0，跳跃40，加速66，二者都96
    /// </summary>
    public static void Extras(byte flag)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = GTA5Mem.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CVehicle);
            GTA5Mem.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_CModelInfo_Extras, flag);
        }
    }

    /// <summary>
    /// 载具降落伞，关0，开1
    /// </summary>
    public static void Parachute(bool isEnable)
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = GTA5Mem.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CVehicle);
            if (isEnable)
                GTA5Mem.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_CModelInfo_Parachute, 0x01);
            else
                GTA5Mem.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_CModelInfo_Parachute, 0x00);
        }
    }

    /// <summary>
    /// 补满载具血量
    /// </summary>
    public static void FillHealth()
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = GTA5Mem.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CVehicle);

            float oVHealth = GTA5Mem.Read<float>(pCVehicle + Offsets.CPed_CVehicle_Health);
            float oVHealthMax = GTA5Mem.Read<float>(pCVehicle + Offsets.CPed_CVehicle_HealthMax);
            if (oVHealth < oVHealthMax)
            {
                GTA5Mem.Write(pCVehicle + Offsets.CPed_CVehicle_Health, oVHealthMax);
            }
            else
            {
                GTA5Mem.Write(pCVehicle + Offsets.CPed_CVehicle_Health, 1000.0f);
            }
        }
    }

    /// <summary>
    /// 修复载具外观
    /// </summary>
    public static void Fix1stfoundBST()
    {
        Task.Run(() =>
        {
            GTA5Mem.Write<int>(General.GlobalPTR + 0x08 * 0x0A, new int[] { 0x17BE28 }, 1);
            GTA5Mem.Write<float>(General.WorldPTR, new int[] { 0x08, 0xD30, 0x280 }, 999.0f);

            Task.Delay(300).Wait();

            int FixVehValue = GTA5Mem.Read<int>(General.PickupDataPTR, new int[] { 0x228 });
            int BSTValue = GTA5Mem.Read<int>(General.PickupDataPTR, new int[] { 0x160 });

            long m_dwpPickUpInterface = GTA5Mem.Read<long>(General.ReplayInterfacePTR, new int[] { 0x20 });
            long dw_curPickUpNum = GTA5Mem.Read<long>(m_dwpPickUpInterface + 0x110, null);
            long m_dwpPedList = GTA5Mem.Read<long>(m_dwpPickUpInterface + 0x100, null);

            for (long i = 0; i < dw_curPickUpNum; i++)
            {
                long dwpPickup = GTA5Mem.Read<long>(m_dwpPedList + i * 0x10, null);
                int dwPickupValue = GTA5Mem.Read<int>(dwpPickup + 0x490, null);

                if (dwPickupValue == BSTValue)
                {
                    GTA5Mem.Write<int>(dwpPickup + 0x490, FixVehValue);

                    Task.Delay(10).Wait();

                    float dwpPickupX = GTA5Mem.Read<float>(dwpPickup + 0x90, null);
                    float dwpPickupY = GTA5Mem.Read<float>(dwpPickup + 0x94, null);
                    float dwpPickupZ = GTA5Mem.Read<float>(dwpPickup + 0x98, null);

                    float Vehx = GTA5Mem.Read<float>(General.WorldPTR, Offsets.VehicleVisualX);
                    float Vehy = GTA5Mem.Read<float>(General.WorldPTR, Offsets.VehicleVisualY);
                    float Vehz = GTA5Mem.Read<float>(General.WorldPTR, Offsets.VehicleVisualZ);

                    Task.Delay(10).Wait();

                    GTA5Mem.Write<float>(dwpPickup + 0x90, Vehx);
                    GTA5Mem.Write<float>(dwpPickup + 0x94, Vehy);
                    GTA5Mem.Write<float>(dwpPickup + 0x98, Vehz);

                    GTA5Mem.Write<float>(General.WorldPTR, new int[] { 0x08, 0xD30, 0x9F8 }, 0);
                }
            }

            Task.Delay(550).Wait();

            Online.InstantBullShark(false);
        });
    }

    /// <summary>
    /// 刷出线上载具
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="z255"></param>
    /// <param name="dist"></param>
    /// <param name="mod"></param>
    public static void SpawnVehicle(long hash, float z255, int dist, int[] mod)
    {
        Task.Run(() =>
        {
            if (hash != 0)
            {
                float x = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerPositionX);
                float y = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerPositionY);
                float z = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerPositionZ);
                float sin = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerSin);
                float cos = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerCos);

                x += cos * dist;
                y += sin * dist;

                if (z255 == -255.0f)
                    z = z255;
                else
                    z += z255;

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 66, hash);   // 载具哈希值

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 94, 2);       // personal car ownerflag  个人载具拥有者标志
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 95, 14);      // ownerflag  拥有者标志

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 5, -1);       // primary -1 auto 159  主色调
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 6, -1);       // secondary -1 auto 159  副色调

                Hacks.WriteGA<float>(Offsets.oVMCreate + 7 + 0, x);       // 载具坐标x
                Hacks.WriteGA<float>(Offsets.oVMCreate + 7 + 1, y);       // 载具坐标y
                Hacks.WriteGA<float>(Offsets.oVMCreate + 7 + 2, z);       // 载具坐标z

                Hacks.WriteGAString(Offsets.oVMCreate + 27 + 1, Guid.NewGuid().ToString()[..8]);    // License plate  车牌

                for (int i = 0; i < 43; i++)
                {
                    if (i < 17)
                    {
                        Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 10 + i, mod[i]);
                    }
                    else if (i >= 17 && i != 42)
                    {
                        Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 10 + 6 + i, mod[i]);
                    }
                    else if (mod[42] > 0 && i == 42)
                    {
                        Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 10 + 6 + 42, new Random().Next(1, mod[42] + 1));
                    }
                }

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 7, -1);       // pearlescent
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 8, -1);       // wheel color
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 33, -1);      // wheel selection
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 69, -1);      // Wheel type

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 28, 1);
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 30, 1);
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 32, 1);
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 65, 1);

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 77, 0xF0400200);         // vehstate  载具状态 没有这个载具起落架是收起状态

                Hacks.WriteGA<int>(Offsets.oVMCreate + 5, 1);                         // can spawn flag must be odd
                Hacks.WriteGA<int>(Offsets.oVMCreate + 2, 1);                         // spawn toggle gets reset to 0 on car spawn
            }
        });
    }

    /// <summary>
    /// 刷出线上载具
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="z255"></param>
    public static void SpawnVehicle(long hash, float z255)
    {
        Task.Run(() =>
        {
            if (hash != 0)
            {
                int dist = 5;

                const int pegasus = 0;

                float x = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerPositionX);
                float y = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerPositionY);
                float z = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerPositionZ);
                float sin = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerSin);
                float cos = GTA5Mem.Read<float>(General.WorldPTR, Offsets.PlayerCos);

                x += cos * dist;
                y += sin * dist;

                if (z255 == -255.0f)
                    z = z255;
                else
                    z += z255;

                Hacks.WriteGA<float>(Offsets.oVMCreate + 7 + 0, x);                   // 载具坐标x
                Hacks.WriteGA<float>(Offsets.oVMCreate + 7 + 1, y);                   // 载具坐标y
                Hacks.WriteGA<float>(Offsets.oVMCreate + 7 + 2, z);                   // 载具坐标z

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 66, hash);               // 载具哈希值
                Hacks.WriteGA<int>(Offsets.oVMCreate + 3, pegasus);                   // 帕格萨斯

                Hacks.WriteGA<int>(Offsets.oVMCreate + 5, 1);                         // can spawn flag must be odd
                Hacks.WriteGA<int>(Offsets.oVMCreate + 2, 1);                         // spawn toggle gets reset to 0 on car spawn
                Hacks.WriteGAString(Offsets.oVMCreate + 27 + 1, Guid.NewGuid().ToString()[..8]);    // License plate  车牌

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 5, -1);       // primary -1 auto 159  主色调
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 6, -1);       // secondary -1 auto 159  副色调
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 7, -1);       // pearlescent
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 8, -1);       // wheel color

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 15, 1);       // primary weapon  主武器
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 19, -1);
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 20, 2);       // secondary weapon  副武器

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 21, 3);       // engine (0-3)  引擎
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 22, 6);       // brakes (0-6)  刹车
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 23, 9);       // transmission (0-9)  变速器
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 24, new Random().Next(0, 78));        // horn (0-77)  喇叭
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 25, 14);      // suspension (0-13)  悬吊系统
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 26, 19);      // armor (0-18)  装甲
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 27, 1);       // turbo (0-1)  涡轮增压
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 28, 1);       // weaponised ownerflag

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 30, 1);
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 32, new Random().Next(0, 15));        // colored light (0-14)
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 33, -1);                              // Wheel Selection

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 60, 1);  // landinggear/vehstate 起落架/载具状态

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 62, new Random().Next(0, 256));       // Tire smoke color R
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 63, new Random().Next(0, 256));       // Green Neon Amount 1-255 100%-0%
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 64, new Random().Next(0, 256));       // Blue Neon Amount 1-255 100%-0%
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 65, new Random().Next(0, 7));         // Window tint 0-6
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 67, 1);       // Livery
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 69, -1);      // Wheel type

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 74, new Random().Next(0, 256));       // Red Neon Amount 1-255 100%-0%
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 75, new Random().Next(0, 256));       // G
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 76, new Random().Next(0, 256));       // B

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 77, 4030726305);                     // vehstate  载具状态 没有这个载具起落架是收起状态
                GTA5Mem.Write<byte>(Hacks.ReadGA<long>(Offsets.oVMCreate + 27 + 77) + 1, 0x02);    // 2:bulletproof 0:false  防弹的

                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 95, 14);      // ownerflag  拥有者标志
                Hacks.WriteGA<int>(Offsets.oVMCreate + 27 + 94, 2);       // personal car ownerflag  个人载具拥有者标志
            }
        });
    }

    /// <summary>
    /// 刷出个人载具
    /// </summary>
    /// <param name="index"></param>
    public static void SpawnPersonalVehicle(int index)
    {
        Hacks.WriteGA<int>(Offsets.SpawnPersonalVehicleIndex1, index);
        Hacks.WriteGA<int>(Offsets.SpawnPersonalVehicleIndex2, 1);
    }

    /// <summary>
    /// 查找载具显示名称
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="isDisplay"></param>
    /// <returns></returns>
    public static string FindVehicleDisplayName(long hash, bool isDisplay)
    {
        foreach (var item in Data.VehicleData.VehicleClassData)
        {
            foreach (var item0 in item.VehicleInfo)
            {
                if (item0.Hash == hash)
                {
                    if (isDisplay)
                        return item0.DisplayName;
                    else
                        return item0.Name;
                }
            }
        }

        return "";
    }
}
