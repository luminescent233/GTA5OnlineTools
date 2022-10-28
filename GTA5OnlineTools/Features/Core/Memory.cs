using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Common.Helper;

namespace GTA5OnlineTools.Features.Core;

public static class Memory
{
    /// <summary>
    /// GTA5进程类
    /// </summary>
    private static Process GTA5Process { get; set; } = null;
    /// <summary>
    /// GTA5窗口句柄
    /// </summary>
    public static IntPtr GTA5WinHandle { get; private set; } = IntPtr.Zero;
    /// <summary>
    /// GTA5进程Id
    /// </summary>
    public static int GTA5ProId { get; private set; } = 0;
    /// <summary>
    /// GTA5主模块基址
    /// </summary>
    public static long GTA5ProBaseAddress { get; private set; } = 0;
    /// <summary>
    /// GTA5进程句柄
    /// </summary>
    public static IntPtr GTA5ProHandle { get; private set; } = IntPtr.Zero;

    /// <summary>
    /// 初始化内存模块
    /// </summary>
    /// <returns></returns>
    public static bool Initialize()
    {
        try
        {
            LoggerHelper.Info("开始初始化《GTA5》内存模块");
            var pArray = Process.GetProcessesByName("GTA5");
            if (pArray.Length > 0)
            {
                LoggerHelper.Info($"《GTA5》进程数量 {pArray.Length}");
                foreach (var item in pArray)
                {
                    if (item.MainWindowHandle == IntPtr.Zero)
                        continue;

                    if (item.MainModule.FileVersionInfo.LegalCopyright == "Rockstar Games Inc. (C) 2005-2022 Take Two Interactive. All rights reserved.")
                    {
                        GTA5Process = item;
                        break;
                    }
                }

                if (GTA5Process == null)
                {
                    LoggerHelper.Error($"发生错误，未找到正确的《GTA5》进程");
                    return false;
                }

                if (GTA5Process.MainWindowHandle == IntPtr.Zero)
                {
                    LoggerHelper.Error($"发生错误，《GTA5》窗口句柄为空");
                    return false;
                }
                GTA5WinHandle = GTA5Process.MainWindowHandle;
                LoggerHelper.Info($"《GTA5》程序窗口句柄 {GTA5WinHandle}");

                if (GTA5Process.Id == 0)
                {
                    LoggerHelper.Error($"发生错误，《GTA5》程序进程ID为空");
                    return false;
                }
                GTA5ProId = GTA5Process.Id;
                LoggerHelper.Info($"《GTA5》程序进程ID {GTA5ProId}");

                if (GTA5Process.MainModule != null)
                {
                    GTA5ProBaseAddress = GTA5Process.MainModule.BaseAddress.ToInt64();
                    LoggerHelper.Info($"《GTA5》程序主模块基址 0x{GTA5ProBaseAddress:x}");

                    GTA5ProHandle = Win32.OpenProcess(ProcessAccessFlags.All, false, GTA5ProId);
                    if (GTA5ProHandle == IntPtr.Zero)
                    {
                        LoggerHelper.Error($"发生错误，《GTA5》程序进程句柄为空");
                        return false;
                    }
                    LoggerHelper.Info($"《GTA5》程序进程句柄 {GTA5ProHandle}");
                    return true;
                }
                else
                {
                    LoggerHelper.Error($"发生错误，《GTA5》程序主模块基址为空");
                    return false;
                }
            }
            else
            {
                LoggerHelper.Error($"发生错误，未发现《GTA5》进程");
                return false;
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("《GTA5》内存模块初始化异常", ex);
            return false;
        }
    }

    /// <summary>
    /// 关闭GTA5进程句柄
    /// </summary>
    public static void CloseHandle()
    {
        if (GTA5ProHandle != IntPtr.Zero)
        {
            Win32.CloseHandle(GTA5ProHandle);

            GTA5Process = null;
            GTA5WinHandle = IntPtr.Zero;
            GTA5ProId = 0;
            GTA5ProBaseAddress = 0;
            GTA5ProHandle = IntPtr.Zero;

            Globals.WorldPTR = 0;
            Globals.BlipPTR = 0;
            Globals.GlobalPTR = 0;
            Globals.NetworkPlayerMgrPTR = 0;
            Globals.ReplayInterfacePTR = 0;
            Globals.WeatherPTR = 0;
            Globals.UnkModelPTR = 0;
            Globals.PickupDataPTR = 0;
            Globals.ViewPortPTR = 0;
            Globals.AimingPedPTR = 0;
            Globals.CCameraPTR = 0;
            Globals.UnkPTR = 0;
            Globals.LocalScriptsPTR = 0;
        }
    }

    /// <summary>
    /// GTA5特征码寻址
    /// </summary>
    public static void PatternInit()
    {
        if (Globals.WorldPTR == 0)
        {
            Globals.WorldPTR = FindPattern(Offsets.Mask.WorldMask);
            Globals.WorldPTR = Rip_37(Globals.WorldPTR);
            LoggerHelper.Info($"《GTA5》WorldPTR 0x{Globals.WorldPTR:x}");
        }

        if (Globals.BlipPTR == 0)
        {
            Globals.BlipPTR = FindPattern(Offsets.Mask.BlipMask);
            Globals.BlipPTR = Rip_37(Globals.BlipPTR);
            LoggerHelper.Info($"《GTA5》BlipPTR 0x{Globals.BlipPTR:x}");
        }

        if (Globals.GlobalPTR == 0)
        {
            Globals.GlobalPTR = FindPattern(Offsets.Mask.GlobalMask);
            Globals.GlobalPTR = Rip_37(Globals.GlobalPTR);
            LoggerHelper.Info($"《GTA5》GlobalPTR 0x{Globals.GlobalPTR:x}");
        }

        if (Globals.NetworkPlayerMgrPTR == 0)
        {
            Globals.NetworkPlayerMgrPTR = FindPattern(Offsets.Mask.NetworkPlayerMgrMask);
            Globals.NetworkPlayerMgrPTR = Rip_37(Globals.NetworkPlayerMgrPTR);
            LoggerHelper.Info($"《GTA5》NetworkPlayerMgrPTR 0x{Globals.NetworkPlayerMgrPTR:x}");
        }

        if (Globals.ReplayInterfacePTR == 0)
        {
            Globals.ReplayInterfacePTR = FindPattern(Offsets.Mask.ReplayInterfaceMask);
            Globals.ReplayInterfacePTR = Rip_37(Globals.ReplayInterfacePTR);
            LoggerHelper.Info($"《GTA5》ReplayInterfacePTR 0x{Globals.ReplayInterfacePTR:x}");
        }

        if (Globals.WeatherPTR == 0)
        {
            Globals.WeatherPTR = FindPattern(Offsets.Mask.WeatherMask);
            Globals.WeatherPTR = Rip_6A(Globals.WeatherPTR);
            LoggerHelper.Info($"《GTA5》WeatherPTR 0x{Globals.WeatherPTR:x}");
        }

        if (Globals.UnkModelPTR == 0)
        {
            Globals.UnkModelPTR = FindPattern(Offsets.Mask.UnkModelMask);
            Globals.UnkModelPTR = Rip_37(Globals.UnkModelPTR);
            LoggerHelper.Info($"《GTA5》UnkModelPTR 0x{Globals.UnkModelPTR:x}");
        }

        if (Globals.PickupDataPTR == 0)
        {
            Globals.PickupDataPTR = FindPattern(Offsets.Mask.PickupDataMask);
            Globals.PickupDataPTR = Rip_37(Globals.PickupDataPTR);
            LoggerHelper.Info($"《GTA5》PickupDataPTR 0x{Globals.PickupDataPTR:x}");
        }

        if (Globals.ViewPortPTR == 0)
        {
            Globals.ViewPortPTR = FindPattern(Offsets.Mask.ViewPortMask);
            Globals.ViewPortPTR = Rip_37(Globals.ViewPortPTR);
            LoggerHelper.Info($"《GTA5》ViewPortPTR 0x{Globals.ViewPortPTR:x}");
        }

        if (Globals.AimingPedPTR == 0)
        {
            Globals.AimingPedPTR = FindPattern(Offsets.Mask.AimingPedMask);
            Globals.AimingPedPTR = Rip_37(Globals.AimingPedPTR);
            LoggerHelper.Info($"《GTA5》AimingPedPTR 0x{Globals.AimingPedPTR:x}");
        }

        if (Globals.CCameraPTR == 0)
        {
            Globals.CCameraPTR = FindPattern(Offsets.Mask.CCameraMask);
            Globals.CCameraPTR = Rip_37(Globals.CCameraPTR);
            LoggerHelper.Info($"《GTA5》CCameraPTR 0x{Globals.CCameraPTR:x}");
        }

        if (Globals.UnkPTR == 0)
        {
            Globals.UnkPTR = FindPattern(Offsets.Mask.UnkMask);
            Globals.UnkPTR = Rip_37(Globals.UnkPTR);
            LoggerHelper.Info($"《GTA5》UnkPTR 0x{Globals.UnkPTR:x}");
        }

        if (Globals.LocalScriptsPTR == 0)
        {
            Globals.LocalScriptsPTR = FindPattern(Offsets.Mask.LocalScriptsMask);
            Globals.LocalScriptsPTR = Rip_37(Globals.LocalScriptsPTR);
            LoggerHelper.Info($"《GTA5》LocalScriptsPTR 0x{Globals.LocalScriptsPTR:x}");
        }
    }

    /// <summary>
    /// 判断GTA5窗口是否在最前
    /// </summary>
    public static bool IsForegroundWindow()
    {
        return GTA5WinHandle == Win32.GetForegroundWindow();
    }

    /// <summary>
    /// 将GTA5窗口置于最前方
    /// </summary>
    public static void SetForegroundWindow()
    {
        Win32.SetForegroundWindow(GTA5WinHandle);
    }

    /// <summary>
    /// 是否置顶GTA5窗口
    /// </summary>
    public static void TopMostProcess(bool isTopMost)
    {
        if (isTopMost)
            Win32.SetWindowPos(GTA5WinHandle, -1, 0, 0, 0, 0, 1 | 2);
        else
            Win32.SetWindowPos(GTA5WinHandle, -2, 0, 0, 0, 0, 1 | 2);
    }

    /// <summary>
    /// 获取GTA5窗口数据信息
    /// </summary>
    /// <returns></returns>
    public static WindowData GetGameWindowData()
    {
        // 获取指定窗口句柄的窗口矩形数据和客户区矩形数据
        Win32.GetWindowRect(GTA5WinHandle, out W32RECT windowRect);
        Win32.GetClientRect(GTA5WinHandle, out W32RECT clientRect);

        // 计算窗口区的宽和高
        var windowWidth = windowRect.Right - windowRect.Left;
        var windowHeight = windowRect.Bottom - windowRect.Top;

        // 处理窗口最小化
        if (windowRect.Left == -32000)
        {
            return new WindowData()
            {
                Left = 0,
                Top = 0,
                Width = 1,
                Height = 1
            };
        }

        // 计算客户区的宽和高
        var clientWidth = clientRect.Right - clientRect.Left;
        var clientHeight = clientRect.Bottom - clientRect.Top;

        // 计算边框
        var borderWidth = (windowWidth - clientWidth) / 2;
        var borderHeight = windowHeight - clientHeight - borderWidth;

        return new WindowData()
        {
            Left = windowRect.Left += borderWidth,
            Top = windowRect.Top += borderHeight,
            Width = clientWidth,
            Height = clientHeight
        };
    }

    /// <summary>
    /// 特征码搜索
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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
        Win32.ReadProcessMemory(GTA5ProHandle, GTA5ProBaseAddress, localModulebytes, moduleSize, out _);

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

    public static long Rip_6A(long address)
    {
        return address + Read<int>(address + 0x06) + 0x0A;
    }

    public static long Rip_389(long address)
    {
        return address + Read<int>(address + 0x03) - 0x89;
    }

    /// <summary>
    /// 获取取偏移数组指针
    /// </summary>
    /// <param name="pointer"></param>
    /// <param name="offset"></param>
    private static long GetPtrAddress(long pointer, int[] offset)
    {
        if (offset != null)
        {
            var buffer = new byte[8];
            Win32.ReadProcessMemory(GTA5ProHandle, pointer, buffer, buffer.Length, out _);

            for (int i = 0; i < (offset.Length - 1); i++)
            {
                pointer = BitConverter.ToInt64(buffer, 0) + offset[i];
                Win32.ReadProcessMemory(GTA5ProHandle, pointer, buffer, buffer.Length, out _);
            }

            pointer = BitConverter.ToInt64(buffer, 0) + offset[offset.Length - 1];
        }

        return pointer;
    }

    /// <summary>
    /// 泛型读取内存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public static T Read<T>(long address) where T : struct
    {
        var buffer = new byte[Marshal.SizeOf(typeof(T))];
        Win32.ReadProcessMemory(GTA5ProHandle, address, buffer, buffer.Length, out _);
        return ByteArrayToStructure<T>(buffer);
    }

    /// <summary>
    /// 泛型读取内存，带偏移数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="basePtr"></param>
    /// <param name="offsets"></param>
    public static T Read<T>(long basePtr, int[] offsets) where T : struct
    {
        var buffer = new byte[Marshal.SizeOf(typeof(T))];
        Win32.ReadProcessMemory(GTA5ProHandle, GetPtrAddress(basePtr, offsets), buffer, buffer.Length, out _);
        return ByteArrayToStructure<T>(buffer);
    }

    /// <summary>
    /// 泛型写入内存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="basePtr"></param>
    /// <param name="offsets"></param>
    /// <param name="value"></param>
    public static void Write<T>(long address, T value) where T : struct
    {
        var buffer = StructureToByteArray(value);
        Win32.WriteProcessMemory(GTA5ProHandle, address, buffer, buffer.Length, out _);
    }

    /// <summary>
    /// 泛型写入内存，带偏移数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <param name="value"></param>
    public static void Write<T>(long basePtr, int[] offsets, T value) where T : struct
    {
        var buffer = StructureToByteArray(value);
        Win32.WriteProcessMemory(GTA5ProHandle, GetPtrAddress(basePtr, offsets), buffer, buffer.Length, out _);
    }

    /// <summary>
    /// 读取矩阵数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <param name="MatrixSize"></param>
    /// <returns></returns>
    public static float[] ReadMatrix<T>(long address, int MatrixSize) where T : struct
    {
        var ByteSize = Marshal.SizeOf(typeof(T));
        var buffer = new byte[ByteSize * MatrixSize];
        Win32.ReadProcessMemory(GTA5ProHandle, address, buffer, buffer.Length, out _);
        return ConvertToFloatArray(buffer);
    }

    /// <summary>
    /// 读取字符串
    /// </summary>
    /// <param name="address"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string ReadString(long address, int size)
    {
        var buffer = new byte[size];
        Win32.ReadProcessMemory(GTA5ProHandle, address, buffer, size, out _);

        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] == 0)
            {
                byte[] _buffer = new byte[i];
                Buffer.BlockCopy(buffer, 0, _buffer, 0, i);
                return Encoding.ASCII.GetString(_buffer);
            }
        }

        return Encoding.ASCII.GetString(buffer);
    }

    /// <summary>
    /// 读取字符串，带偏移数组
    /// </summary>
    /// <param name="basePtr"></param>
    /// <param name="offsets"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string ReadString(long basePtr, int[] offsets, int size)
    {
        var buffer = new byte[size];
        Win32.ReadProcessMemory(GTA5ProHandle, GetPtrAddress(basePtr, offsets), buffer, size, out _);

        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] == 0)
            {
                var _buffer = new byte[i];
                Buffer.BlockCopy(buffer, 0, _buffer, 0, i);
                return Encoding.UTF8.GetString(_buffer);
            }
        }

        return Encoding.UTF8.GetString(buffer);
    }

    /// <summary>
    /// 写入字符串
    /// </summary>
    /// <param name="address"></param>
    /// <param name="str"></param>
    public static void WriteString(long address,  string str)
    {
        var buffer = new ASCIIEncoding().GetBytes(str);
        Win32.WriteProcessMemory(GTA5ProHandle, address, buffer, buffer.Length, out _);
    }

    /// <summary>
    /// 写入字符串，带偏移数组
    /// </summary>
    /// <param name="basePtr"></param>
    /// <param name="offsets"></param>
    /// <param name="str"></param>
    public static void WriteString(long basePtr, int[] offsets, string str)
    {
        var buffer = new ASCIIEncoding().GetBytes(str);
        Win32.WriteProcessMemory(GTA5ProHandle, GetPtrAddress(basePtr, offsets), buffer, buffer.Length, out _);
    }

    /// <summary>
    /// 读取字节
    /// </summary>
    /// <param name="basePtr"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static byte[] ReadBytes(long basePtr, int size)
    {
        var buffer = new byte[size];

        Win32.ReadProcessMemory(GTA5ProHandle, basePtr, buffer, size, out _);
        return buffer;
    }

    /// <summary>
    /// 写入字节
    /// </summary>
    /// <param name="basePtr"></param>
    /// <param name="bytes"></param>
    public static void WriteBytes(long basePtr, byte[] bytes)
    {
        Win32.WriteProcessMemory(GTA5ProHandle, basePtr, bytes, bytes.Length, out _);
    }

    //////////////////////////////////////////////////////////////////

    /// <summary>
    /// 判断内存地址是否有效
    /// </summary>
    /// <param name="Address"></param>
    public static bool IsValid(long Address)
    {
        return Address >= 0x10000 && Address < 0x000F000000000000;
    }

    /// <summary>
    /// 字节数组转结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 结构转字节数组
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static byte[] StructureToByteArray(object obj)
    {
        var length = Marshal.SizeOf(obj);
        var array = new byte[length];
        IntPtr pointer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(obj, pointer, true);
        Marshal.Copy(pointer, array, 0, length);
        Marshal.FreeHGlobal(pointer);
        return array;
    }

    /// <summary>
    /// 转换浮点型数组
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static float[] ConvertToFloatArray(byte[] bytes)
    {
        if (bytes.Length % 4 != 0)
        {
            throw new ArgumentException();
        }

        var floats = new float[bytes.Length / 4];
        for (int i = 0; i < floats.Length; i++)
        {
            floats[i] = BitConverter.ToSingle(bytes, i * 4);
        }
        return floats;
    }
}

public struct WindowData
{
    public int Left;
    public int Top;
    public int Width;
    public int Height;
}
