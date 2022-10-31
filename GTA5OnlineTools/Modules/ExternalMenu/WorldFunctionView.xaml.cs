﻿using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Common.Helper;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Data;
using GTA5OnlineTools.Features.Client;

namespace GTA5OnlineTools.Modules.ExternalMenu;

/// <summary>
/// WorldFunctionView.xaml 的交互逻辑
/// </summary>
public partial class WorldFunctionView : UserControl
{
    /// <summary>
    /// 临时传送坐标
    /// </summary>
    private Vector3 tempVector3 = Vector3.Zero;

    public WorldFunctionView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;

        // 如果配置文件不存在就创建
        if (!File.Exists(FileUtil.F_CustomTPList_Path))
        {
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(FileUtil.F_CustomTPList_Path))
        {
            using var streamReader = new StreamReader(FileUtil.F_CustomTPList_Path);
            List<TeleportData.TeleportInfo> teleportPreviews = JsonUtil.JsonDese<List<TeleportData.TeleportInfo>>(streamReader.ReadToEnd());

            TeleportData.CustomTeleport.Clear();

            foreach (var item in teleportPreviews)
            {
                TeleportData.CustomTeleport.Add(item);
            }
        }

        foreach (var item in TeleportData.TeleportClassData)
        {
            ComboBox_TeleportClass.Items.Add(new EmojiMenu()
            {
                Emoji = item.Emoji,
                Title = item.Name
            });
        }
        ComboBox_TeleportClass.SelectedIndex = 0;
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        // 写入到Json文件
        File.WriteAllText(FileUtil.F_CustomTPList_Path, JsonUtil.JsonSeri(TeleportData.CustomTeleport));
    }

    private void Button_LocalWeather_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        var str = (e.OriginalSource as Button).Content.ToString();
        var index = MiscData.LocalWeathers.FindIndex(t => t.Name == str);
        if (index != -1)
        {
            World.Set_Local_Weather(MiscData.LocalWeathers[index].ID);
        }
    }

    private void Button_KillNPC_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.KillAllNPC(false);
    }
    private void Button_KillAllHostilityNPC_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.KillAllNPC(true);
    }

    private void Button_KillAllPolice_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.KillAllPolice();
    }

    private void Button_DestroyAllVehicles_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.DestroyAllVehicles();
    }

    private void Button_DestroyAllNPCVehicles_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.DestroyAllNPCVehicles(false);
    }

    private void Button_DestroyAllHostilityNPCVehicles_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.DestroyAllNPCVehicles(true);
    }

    private void Button_TPAllNPCToMe_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.TeleportAllNPCToMe(false);
    }

    private void Button_TPHostilityNPCToMe_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        World.TeleportAllNPCToMe(true);
    }

    /////////////////////////////////////////////////////////////////////////////

    #region 自定义传送
    private void UpdateCustonTeleportList()
    {
        ListBox_TeleportInfo.Items.Clear();

        foreach (var item in TeleportData.TeleportClassData[0].TeleportInfo)
        {
            ListBox_TeleportInfo.Items.Add(item.Name);
        }

        ListBox_TeleportInfo.SelectedIndex = 0;
    }

    private void ComboBox_TeleportClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ComboBox_TeleportClass.SelectedIndex;
        if (index != -1)
        {
            ListBox_TeleportInfo.Items.Clear();

            foreach (var item in TeleportData.TeleportClassData[index].TeleportInfo)
            {
                ListBox_TeleportInfo.Items.Add(item.Name);
            }

            ListBox_TeleportInfo.SelectedIndex = 0;
        }
    }

    private void ListBox_TeleportInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index1 = ComboBox_TeleportClass.SelectedIndex;
        var index2 = ListBox_TeleportInfo.SelectedIndex;
        if (index1 != -1 && index2 != -1)
        {
            tempVector3 = TeleportData.TeleportClassData[index1].TeleportInfo[index2].Position;

            TextBox_CustomTeleportName.Text = TeleportData.TeleportClassData[index1].TeleportInfo[index2].Name;
            TextBox_Position_X.Text = $"{tempVector3.X:0.000}";
            TextBox_Position_Y.Text = $"{tempVector3.Y:0.000}";
            TextBox_Position_Z.Text = $"{tempVector3.Z:0.000}";
        }
        else if (index2 == -1)
        {
            TextBox_CustomTeleportName.Clear();
            TextBox_Position_X.Clear();
            TextBox_Position_Y.Clear();
            TextBox_Position_Z.Clear();
        }
    }

    private void Button_AddCustomTeleport_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Vector3 vector3 = Teleport.GetPlayerPosition();

        TeleportData.CustomTeleport.Add(new TeleportData.TeleportInfo()
        {
            Name = $"保存点 : {DateTime.Now:yyyyMMdd_HHmmss_ffff}",
            Position = vector3
        });

        UpdateCustonTeleportList();

        ListBox_TeleportInfo.SelectedIndex = ListBox_TeleportInfo.Items.Count - 1;
    }

    private void Button_EditCustomTeleport_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        try
        {
            var tempName = TextBox_CustomTeleportName.Text.Trim();
            var tempX = TextBox_Position_X.Text.Trim();
            var tempY = TextBox_Position_Y.Text.Trim();
            var tempZ = TextBox_Position_Z.Text.Trim();

            if (string.IsNullOrEmpty(tempName) ||
                string.IsNullOrEmpty(tempX) || string.IsNullOrEmpty(tempY) || string.IsNullOrEmpty(tempZ))
            {
                NotifierHelper.Show(NotifierType.Warning, "部分坐标数据不能为空");
                return;
            }

            int index1 = ComboBox_TeleportClass.SelectedIndex;
            int index2 = ListBox_TeleportInfo.SelectedIndex;
            if (index1 == 0 && index2 != -1)
            {
                TeleportData.TeleportClassData[index1].TeleportInfo[index2].Position = new Vector3()
                {
                    X = Convert.ToSingle(tempX),
                    Y = Convert.ToSingle(tempY),
                    Z = Convert.ToSingle(tempZ)
                };

                TeleportData.TeleportClassData[index1].TeleportInfo[index2].Name = tempName;

                UpdateCustonTeleportList();

                ListBox_TeleportInfo.SelectedIndex = index2; ;
            }
            else
            {
                NotifierHelper.Show(NotifierType.Warning, "当前自定义传送坐标选中项为空");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    private void Button_DeleteCustomTeleport_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        int index1 = ComboBox_TeleportClass.SelectedIndex;
        int index2 = ListBox_TeleportInfo.SelectedIndex;
        if (index1 == 0 && index2 != -1)
        {
            TeleportData.TeleportClassData[index1].TeleportInfo.Remove(TeleportData.TeleportClassData[index1].TeleportInfo[index2]);

            UpdateCustonTeleportList();

            ListBox_TeleportInfo.SelectedIndex = ListBox_TeleportInfo.Items.Count - 1;
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "当前自定义传送坐标选中项为空");
        }
    }

    private void Button_ToWaypoint_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Teleport.ToWaypoint();
    }

    private void Button_ToObjective_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Teleport.ToObjective();
    }

    private void ListBox_TeleportInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        Button_Teleport_Click(null, null);
    }

    private void Button_Teleport_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Teleport.SetTeleportPosition(tempVector3);
    }

    private void Button_SaveCustomTeleport_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        SaveConfig();

        NotifierHelper.Show(NotifierType.Success, $"保存到自定义传送坐标文件成功");
    }
    #endregion
}
