using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Data;

namespace GTA5OnlineTools.Features.SDK;

public static class Teleport
{
    /// <summary>
    /// 传送到导航点
    /// </summary>
    public static void ToWaypoint()
    {
        SetTeleportPosition(WaypointPosition());
    }

    /// <summary>
    /// 传送到目标点
    /// </summary>
    public static void ToObjective()
    {
        SetTeleportPosition(ObjectivePosition());
    }

    /// <summary>
    /// 传送到Blips
    /// </summary>
    public static void ToBlips(int blipID)
    {
        SetTeleportPosition(CustomObjectivePosition(blipID));
    }

    /// <summary>
    /// 获取玩家当前坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetPlayerPosition()
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        return GTA5Mem.Read<Vector3>(pCPed + Offsets.CPed_VisualX);
    }

    /// <summary>
    /// 坐标传送功能
    /// </summary>
    public static void SetTeleportPosition(Vector3 vector3)
    {
        if (vector3 != Vector3.Zero)
        {
            long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
            long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);

            if (GTA5Mem.Read<int>(pCPed + Offsets.CPed_InVehicle) == 0)
            {
                // 玩家不在载具
                long pCNavigation = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CNavigation);
                GTA5Mem.Write(pCPed + Offsets.CPed_VisualX, vector3);
                GTA5Mem.Write(pCNavigation + Offsets.CPed_CNavigation_PositionX, vector3);
            }
            else
            {
                // 玩家在载具
                long pCVehicle = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CVehicle);
                GTA5Mem.Write(pCVehicle + Offsets.CPed_CVehicle_VisualX, vector3);
                long pCNavigation = GTA5Mem.Read<long>(pCVehicle + Offsets.CPed_CVehicle_CNavigation);
                GTA5Mem.Write(pCNavigation + Offsets.CPed_CVehicle_CNavigation_PositionX, vector3);
            }
        }
    }

    /// <summary>
    /// 获取导航点坐标
    /// </summary>
    public static Vector3 WaypointPosition()
    {
        Vector3 vector3 = Vector3.Zero;
        int dwIcon, dwColor;

        for (int i = 2000; i > 1; i--)
        {
            long pBlip = GTA5Mem.Read<long>(General.BlipPTR + i * 0x08);
            if (pBlip <= 0)
                continue;

            dwIcon = GTA5Mem.Read<int>(pBlip + 0x40);
            dwColor = GTA5Mem.Read<int>(pBlip + 0x48);

            if (dwIcon == 8 && dwColor == 84)
            {
                vector3 = GTA5Mem.Read<Vector3>(pBlip + 0x10);
                vector3.Z = vector3.Z == 20.0f ? -225.0f : vector3.Z + 1.0f;

                return vector3;
            }
        }

        return vector3;
    }

    /// <summary>
    /// 获取目标点坐标
    /// </summary>
    public static Vector3 ObjectivePosition()
    {
        Vector3 vector3 = Vector3.Zero;
        int dwIcon, dwColor;

        for (int i = 2000; i > 1; i--)
        {
            long pBlip = GTA5Mem.Read<long>(General.BlipPTR + i * 0x08);

            dwIcon = GTA5Mem.Read<int>(pBlip + 0x40);
            dwColor = GTA5Mem.Read<int>(pBlip + 0x48);

            if (dwIcon == 1 &&
                (dwColor == 5 || dwColor == 60 || dwColor == 66))
            {
                vector3 = GTA5Mem.Read<Vector3>(pBlip + 0x10);
                vector3.Z += +1.0f;

                return vector3;
            }
        }

        for (int i = 2000; i > 1; i--)
        {
            long pBlip = GTA5Mem.Read<long>(General.BlipPTR + i * 0x08);

            dwIcon = GTA5Mem.Read<int>(pBlip + 0x40);
            dwColor = GTA5Mem.Read<int>(pBlip + 0x48);

            if ((dwIcon == 1 || dwIcon == 225 || dwIcon == 427 || dwIcon == 478 || dwIcon == 501 || dwIcon == 523 || dwIcon == 556) &&
                (dwColor == 1 || dwColor == 2 || dwColor == 3 || dwColor == 54 || dwColor == 78))
            {
                vector3 = GTA5Mem.Read<Vector3>(pBlip + 0x10);
                vector3.Z += +1.0f;

                return vector3;
            }
        }

        for (int i = 2000; i > 1; i--)
        {
            long pBlip = GTA5Mem.Read<long>(General.BlipPTR + i * 0x08);

            dwIcon = GTA5Mem.Read<int>(pBlip + 0x40);
            dwColor = GTA5Mem.Read<int>(pBlip + 0x48);

            if ((dwIcon == 432 || dwIcon == 443) &&
                (dwColor == 59))
            {
                vector3 = GTA5Mem.Read<Vector3>(pBlip + 0x10);
                vector3.Z += +1.0f;

                return vector3;
            }
        }

        return vector3;
    }

    /// <summary>
    /// 获取自定义目标点坐标
    /// </summary>
    public static Vector3 CustomObjectivePosition(int blipID)
    {
        Vector3 vector3 = Vector3.Zero;
        int dwIcon;

        for (int i = 2000; i > 1; i--)
        {
            long pBlip = GTA5Mem.Read<long>(General.BlipPTR + i * 0x08);

            dwIcon = GTA5Mem.Read<int>(pBlip + 0x40);

            if (dwIcon == blipID)
            {
                vector3 = GTA5Mem.Read<Vector3>(pBlip + 0x10);
                vector3.Z = vector3.Z == 20.0f ? -225.0f : vector3.Z + 1.0f;

                return vector3;
            }
        }

        return vector3;
    }

    /// <summary>
    /// 坐标向前微调
    /// </summary>
    public static void MovingFoward()
    {
        long pCPedFactory = GTA5Mem.Read<long>(General.WorldPTR);
        long pCPed = GTA5Mem.Read<long>(pCPedFactory + Offsets.CPed);
        long pCNavigation = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CNavigation);

        float sin = GTA5Mem.Read<float>(pCNavigation + Offsets.CPed_CNavigation_RightX);
        float cos = GTA5Mem.Read<float>(pCNavigation + Offsets.CPed_CNavigation_ForwardX);

        if (GTA5Mem.Read<int>(pCPed + Offsets.CPed_InVehicle) == 0)
        {
            // 玩家不在载具

            float x = GTA5Mem.Read<float>(pCPed + Offsets.CPed_VisualX);
            float y = GTA5Mem.Read<float>(pCPed + Offsets.CPed_VisualY);

            x += Settings.Forward * cos;
            y += Settings.Forward * sin;

            GTA5Mem.Write(pCPed + Offsets.CPed_VisualX, x);
            GTA5Mem.Write(pCPed + Offsets.CPed_VisualY, y);
            GTA5Mem.Write(pCNavigation + Offsets.CPed_CNavigation_PositionX, x);
            GTA5Mem.Write(pCNavigation + Offsets.CPed_CNavigation_PositionY, y);
        }
        else
        {
            // 玩家在载具
            long pCVehicle = GTA5Mem.Read<long>(pCPed + Offsets.CPed_CVehicle);
            long pCVehicle_CNavigation = GTA5Mem.Read<long>(pCVehicle + Offsets.CPed_CVehicle_CNavigation);

            float x = GTA5Mem.Read<float>(pCVehicle + Offsets.CPed_CVehicle_VisualX);
            float y = GTA5Mem.Read<float>(pCVehicle + Offsets.CPed_CVehicle_VisualY);

            x += Settings.Forward * cos;
            y += Settings.Forward * sin;

            GTA5Mem.Write(pCVehicle + Offsets.CPed_CVehicle_VisualX, x);
            GTA5Mem.Write(pCVehicle + Offsets.CPed_CVehicle_VisualY, y);
            GTA5Mem.Write(pCVehicle_CNavigation + Offsets.CPed_CVehicle_CNavigation_PositionX, x);
            GTA5Mem.Write(pCVehicle_CNavigation + Offsets.CPed_CVehicle_CNavigation_PositionY, y);
        }
    }
}
