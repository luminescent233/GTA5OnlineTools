using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Client;

namespace GTA5OnlineTools.Features.SDK;

public static class Vehicle
{
    /// <summary>
    /// 玩家是否在载具中
    /// </summary>
    public static bool IsInVehicle()
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        return oInVehicle == 0x01;
    }

    /// <summary>
    /// 载具无敌模式
    /// </summary>
    public static void GodMode(bool isEnable)
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = Memory.Read<long>(pCPed + Offsets.CPed_CVehicle);
            if (isEnable)
                Memory.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_God, 0x01);
            else
                Memory.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_God, 0x00);
        }
    }

    /// <summary>
    /// 载具安全带
    /// </summary>
    public static void Seatbelt(bool isEnable)
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);

        if (isEnable)
            Memory.Write<byte>(pCPed + Offsets.CPed_Seatbelt, 0xC9);
        else
            Memory.Write<byte>(pCPed + Offsets.CPed_Seatbelt, 0xC8);
    }

    /// <summary>
    /// 载具隐形
    /// </summary>
    public static void Invisible(bool isEnable)
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = Memory.Read<long>(pCPed + Offsets.CPed_CVehicle);

            if (isEnable)
                Memory.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_Invisible, 0x01);
            else
                Memory.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_Invisible, 0x27);
        }
    }

    /// <summary>
    /// 载具附加功能
    /// </summary>
    public static void Extras(short flag)
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = Memory.Read<long>(pCPed + Offsets.CPed_CVehicle);
            long pCModelInfo = Memory.Read<long>(pCVehicle + Offsets.CPed_CVehicle_CModelInfo);
            Memory.Write(pCModelInfo + Offsets.CPed_CVehicle_CModelInfo_Extras, flag);
        }
    }

    /// <summary>
    /// 载具降落伞，关0，开1
    /// </summary>
    public static void Parachute(bool isEnable)
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = Memory.Read<long>(pCPed + Offsets.CPed_CVehicle);
            long pCModelInfo = Memory.Read<long>(pCVehicle + Offsets.CPed_CVehicle_CModelInfo);
            if (isEnable)
                Memory.Write<byte>(pCModelInfo + Offsets.CPed_CVehicle_CModelInfo_Parachute, 0x01);
            else
                Memory.Write<byte>(pCModelInfo + Offsets.CPed_CVehicle_CModelInfo_Parachute, 0x00);
        }
    }

    /// <summary>
    /// 补满载具血量
    /// </summary>
    public static void FillHealth()
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
        byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

        if (oInVehicle == 0x01)
        {
            long pCVehicle = Memory.Read<long>(pCPed + Offsets.CPed_CVehicle);

            float oVHealth = Memory.Read<float>(pCVehicle + Offsets.CPed_CVehicle_Health);
            float oVHealthMax = Memory.Read<float>(pCVehicle + Offsets.CPed_CVehicle_HealthMax);
            if (oVHealth <= oVHealthMax)
            {
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_Health, oVHealthMax);
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_HealthBody, oVHealthMax);
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_HealthPetrolTank, oVHealthMax);
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_HealthEngine, oVHealthMax);
            }
            else
            {
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_Health, 1000.0f);
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_HealthBody, 1000.0f);
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_HealthPetrolTank, 1000.0f);
                Memory.Write(pCVehicle + Offsets.CPed_CVehicle_HealthEngine, 1000.0f);
            }
        }
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
                long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
                long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
                Vector3 vector3 = Memory.Read<Vector3>(pCPed + Offsets.CPed_VisualX);

                long pCNavigation = Memory.Read<long>(pCPed + Offsets.CPed_CNavigation);

                float sin = Memory.Read<float>(pCNavigation + Offsets.CPed_CNavigation_RightX);
                float cos = Memory.Read<float>(pCNavigation + Offsets.CPed_CNavigation_ForwardX);

                vector3.X += cos * dist;
                vector3.Y += sin * dist;

                if (z255 == -255.0f)
                    vector3.Z = z255;
                else
                    vector3.Z += z255;

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 66, hash);       // 载具哈希值

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 94, 2);          // personal car ownerflag  个人载具拥有者标志
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 95, 14);         // ownerflag  拥有者标志

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 5, -1);          // primary -1 auto 159  主色调
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 6, -1);          // secondary -1 auto 159  副色调

                Hacks.WriteGA(Offsets.oVMCreate + 7 + 0, vector3.X);    // 载具坐标x
                Hacks.WriteGA(Offsets.oVMCreate + 7 + 1, vector3.Y);    // 载具坐标y
                Hacks.WriteGA(Offsets.oVMCreate + 7 + 2, vector3.Z);    // 载具坐标z

                Hacks.WriteGAString(Offsets.oVMCreate + 27 + 1, Guid.NewGuid().ToString()[..8]);    // License plate  车牌

                for (int i = 0; i < 43; i++)
                {
                    if (i < 17)
                    {
                        Hacks.WriteGA(Offsets.oVMCreate + 27 + 10 + i, mod[i]);
                    }
                    else if (i >= 17 && i != 42)
                    {
                        Hacks.WriteGA(Offsets.oVMCreate + 27 + 10 + 6 + i, mod[i]);
                    }
                    else if (mod[42] > 0 && i == 42)
                    {
                        Hacks.WriteGA(Offsets.oVMCreate + 27 + 10 + 6 + 42, new Random().Next(1, mod[42] + 1));
                    }
                }

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 7, -1);      // pearlescent
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 8, -1);      // wheel color
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 33, -1);     // wheel selection
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 69, -1);     // Wheel type

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 28, 1);
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 30, 1);
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 32, 1);
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 65, 1);

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 77, 0xF0400200);   // vehstate  载具状态 没有这个载具起落架是收起状态

                Hacks.WriteGA(Offsets.oVMCreate + 5, 1);                        // can spawn flag must be odd
                Hacks.WriteGA(Offsets.oVMCreate + 2, 1);                        // spawn toggle gets reset to 0 on car spawn
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

                long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
                long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
                Vector3 vector3 = Memory.Read<Vector3>(pCPed + Offsets.CPed_VisualX);

                long pCNavigation = Memory.Read<long>(pCPed + Offsets.CPed_CNavigation);

                float sin = Memory.Read<float>(pCNavigation + Offsets.CPed_CNavigation_RightX);
                float cos = Memory.Read<float>(pCNavigation + Offsets.CPed_CNavigation_ForwardX);

                vector3.X += cos * dist;
                vector3.Y += sin * dist;

                if (z255 == -255.0f)
                    vector3.Z = z255;
                else
                    vector3.Z += z255;

                Hacks.WriteGA(Offsets.oVMCreate + 7 + 0, vector3.X);            // 载具坐标x
                Hacks.WriteGA(Offsets.oVMCreate + 7 + 1, vector3.Y);            // 载具坐标y
                Hacks.WriteGA(Offsets.oVMCreate + 7 + 2, vector3.Z);            // 载具坐标z

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 66, hash);               // 载具哈希值
                Hacks.WriteGA(Offsets.oVMCreate + 3, pegasus);                  // 帕格萨斯

                Hacks.WriteGA(Offsets.oVMCreate + 5, 1);                        // can spawn flag must be odd
                Hacks.WriteGA(Offsets.oVMCreate + 2, 1);                        // spawn toggle gets reset to 0 on car spawn
                Hacks.WriteGAString(Offsets.oVMCreate + 27 + 1, Guid.NewGuid().ToString()[..8]);    // License plate  车牌

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 5, -1);          // primary -1 auto 159  主色调
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 6, -1);          // secondary -1 auto 159  副色调
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 7, -1);          // pearlescent
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 8, -1);          // wheel color

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 15, 1);          // primary weapon  主武器
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 19, -1);
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 20, 2);          // secondary weapon  副武器

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 21, 3);          // engine (0-3)  引擎
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 22, 6);          // brakes (0-6)  刹车
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 23, 9);          // transmission (0-9)  变速器
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 24, new Random().Next(0, 78));        // horn (0-77)  喇叭
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 25, 14);         // suspension (0-13)  悬吊系统
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 26, 19);         // armor (0-18)  装甲
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 27, 1);          // turbo (0-1)  涡轮增压
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 28, 1);          // weaponised ownerflag

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 30, 1);
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 32, new Random().Next(0, 15));       // colored light (0-14)
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 33, -1);                             // Wheel Selection

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 60, 1);    // landinggear/vehstate 起落架/载具状态

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 62, new Random().Next(0, 256));      // Tire smoke color R
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 63, new Random().Next(0, 256));      // Green Neon Amount 1-255 100%-0%
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 64, new Random().Next(0, 256));      // Blue Neon Amount 1-255 100%-0%
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 65, new Random().Next(0, 7));        // Window tint 0-6
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 67, 1);          // Livery
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 69, -1);         // Wheel type

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 74, new Random().Next(0, 256));      // Red Neon Amount 1-255 100%-0%
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 75, new Random().Next(0, 256));      // G
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 76, new Random().Next(0, 256));      // B

                Hacks.WriteGA<long>(Offsets.oVMCreate + 27 + 77, 4030726305);                       // vehstate  载具状态 没有这个载具起落架是收起状态
                Memory.Write<byte>(Hacks.ReadGA<long>(Offsets.oVMCreate + 27 + 77) + 1, 0x02);      // 2:bulletproof 0:false  防弹的

                Hacks.WriteGA(Offsets.oVMCreate + 27 + 95, 14);         // ownerflag  拥有者标志
                Hacks.WriteGA(Offsets.oVMCreate + 27 + 94, 2);          // personal car ownerflag  个人载具拥有者标志
            }
        });
    }

    /// <summary>
    /// 刷出个人载具
    /// </summary>
    /// <param name="index"></param>
    public static void SpawnPersonalVehicle(int index)
    {
        Hacks.WriteGA(Offsets.SpawnPersonalVehicleIndex1, index);
        Hacks.WriteGA(Offsets.SpawnPersonalVehicleIndex2, 1);
    }

    /// <summary>
    /// 查找载具显示名称
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="isDisplay"></param>
    /// <returns></returns>
    public static string FindVehicleDisplayName(long hash, bool isDisplay)
    {
        foreach (var item in VehicleData.VehicleClassData)
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

        return string.Empty;
    }
}
