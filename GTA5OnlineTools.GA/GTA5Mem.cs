using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GTA5OnlineTools.GA;

public static class GTA5Mem
{
    private static Process GTA5Process { get; set; } = null;
    public static IntPtr GTA5WinHandle { get; private set; } = IntPtr.Zero;
    public static int GTA5ProId { get; private set; } = 0;
    public static long GTA5ProBaseAddress { get; private set; } = 0;
    public static IntPtr GTA5ProHandle { get; private set; } = IntPtr.Zero;

    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, [In, Out] byte[] lpBuffer, int nsize, out IntPtr lpNumberOfBytesRead);

    public static bool Initialize()
    {
        try
        {
            GTA5Process = Process.GetProcessesByName("GTA5")[0];
            GTA5WinHandle = GTA5Process.MainWindowHandle;
            GTA5ProId = GTA5Process.Id;
            GTA5ProBaseAddress = GTA5Process.MainModule.BaseAddress.ToInt64();
            GTA5ProHandle = GTA5Process.Handle;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static long FindPattern(string pattern)
    {
        long address = 0;

        var tempArray = new List<byte>();
        foreach (var each in pattern.Split(' '))
        {
            if (each == "??")
            {
                tempArray.Add(Convert.ToByte("0", 16));
            }
            else
            {
                tempArray.Add(Convert.ToByte(each, 16));
            }
        }

        var patternByteArray = tempArray.ToArray();

        var moduleSize = GTA5Process.MainModule.ModuleMemorySize;
        if (moduleSize == 0) throw new Exception($"模块 {GTA5Process.MainModule.ModuleName} 大小无效");

        var localModulebytes = new byte[moduleSize];
        ReadProcessMemory(GTA5ProHandle, GTA5ProBaseAddress, localModulebytes, moduleSize, out _);

        for (int indexAfterBase = 0; indexAfterBase < localModulebytes.Length; indexAfterBase++)
        {
            bool noMatch = false;

            if (localModulebytes[indexAfterBase] != patternByteArray[0])
                continue;

            for (var MatchedIndex = 0; MatchedIndex < patternByteArray.Length && indexAfterBase + MatchedIndex < localModulebytes.Length; MatchedIndex++)
            {
                if (patternByteArray[MatchedIndex] == 0x0)
                    continue;
                if (patternByteArray[MatchedIndex] != localModulebytes[indexAfterBase + MatchedIndex])
                {
                    noMatch = true;
                    break;
                }
            }

            if (!noMatch)
                return GTA5ProBaseAddress + indexAfterBase;
        }

        return address;
    }

    public static long Rip_37(long address)
    {
        return address + Read<int>(address + 0x03) + 0x07;
    }

    public static T Read<T>(long address) where T : struct
    {
        var buffer = new byte[Marshal.SizeOf(typeof(T))];
        ReadProcessMemory(GTA5ProHandle, address, buffer, buffer.Length, out _);
        return ByteArrayToStructure<T>(buffer);
    }

    //////////////////////////////////////////////////////////////////

    public static bool IsValid(long Address)
    {
        return Address >= 0x10000 && Address < 0x000F000000000000;
    }

    private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
    {
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        try
        {
            var obj = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            if (obj != null)
                return (T)obj;
            else
                return default;
        }
        finally
        {
            handle.Free();
        }
    }
}
