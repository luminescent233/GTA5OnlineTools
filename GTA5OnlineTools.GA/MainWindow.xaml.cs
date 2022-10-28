using System;
using System.Windows;
using System.ComponentModel;

namespace GTA5OnlineTools.GA;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    private const string GlobalMask = "4C 8D 05 ?? ?? ?? ?? 4D 8B 08 4D 85 C9 74 11";

    private long GlobalPTR = 0;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        if (GTA5Mem.Initialize())
        {
            GlobalPTR = GTA5Mem.FindPattern(GlobalMask);
            GlobalPTR = GTA5Mem.Rip_37(GlobalPTR);
        }
        else
        {
            MsgBoxUtil.Error("《GTA5》进程异常，程序关闭！");
            Application.Current.Shutdown();
            return;
        }
    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {

    }

    private void Button_Search_Click(object sender, RoutedEventArgs e)
    {
        ListBox_SearchResult.Items.Clear();

        try
        {
            var global = int.Parse(TextBox_Global.Text.Trim());
            var offset_min = int.Parse(TextBox_Offset_Min.Text.Trim());
            var offset_max = int.Parse(TextBox_Offset_Max.Text.Trim());
            var value_min = int.Parse(TextBox_Value_Min.Text.Trim());
            var value_max = int.Parse(TextBox_Value_Max.Text.Trim());

            if (global <= 0)
            {
                MsgBoxUtil.Warning("Global 全局变量不正确");
                return;
            }

            if (offset_min < 0 || offset_max < 0 || offset_min > offset_max)
            {
                MsgBoxUtil.Warning("Offset 偏移范围不正确");
                return;
            }

            if (value_min < 0 || value_max < 0 || value_min > value_max)
            {
                MsgBoxUtil.Warning("Value 结果范围不正确");
                return;
            }

            int count = offset_max - offset_min;
            for (int i = 0; i <= count; i++)
            {
                var value = ReadGA<int>(global + offset_min + i);

                if (value_min == value_max && value == value_min)
                {
                    ListBox_SearchResult.Items.Add($"Global_{global}.f_{offset_min + i} = {value}");
                }
                else if (value >= value_min && value <= value_max)
                {
                    ListBox_SearchResult.Items.Add($"Global_{global}.f_{offset_min + i} = {value}");
                }
            }

            MsgBoxUtil.Information("搜索完成");
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    //////////////////////////////////////////////////////////////

    private long GlobalAddress(int index)
    {
        return GTA5Mem.Read<long>(GlobalPTR + 0x8 * ((index >> 0x12) & 0x3F)) + 8 * (index & 0x3FFFF);
    }

    private T ReadGA<T>(int index) where T : struct
    {
        return GTA5Mem.Read<T>(GlobalAddress(index));
    }
}
