using GTA5OnlineTools.Features.Core;

namespace GTA5OnlineTools.Features.SDK;

public static class Globals
{
    /// <summary>
    /// 获取 CPed 指针
    /// </summary>
    /// <returns></returns>
    public static long GetCPed()
    {
        long pCPedFactory = Memory.Read<long>(Pointers.WorldPTR);
        return Memory.Read<long>(pCPedFactory + Offsets.CPed);
    }

    /// <summary>
    /// 获取 CPlayerInfo 指针
    /// </summary>
    /// <returns></returns>
    public static long GetCPlayerInfo()
    {
        long pCPed = GetCPed();
        return Memory.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);
    }
}
