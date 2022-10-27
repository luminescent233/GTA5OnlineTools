using GTA5OnlineTools.Common.Utils;
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

        UpdateTeleportList();

        ListBox_TeleportClass.SelectedIndex = 0;
        ListBox_TeleportInfo.SelectedIndex = 0;
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
    /// <summary>
    /// 更新传送列表
    /// </summary>
    private void UpdateTeleportList()
    {
        ListBox_TeleportClass.Items.Clear();

        // 传送列表
        foreach (var item in TeleportData.TeleportDataClass)
        {
            ListBox_TeleportClass.Items.Add(item.ClassName);
        }

        ListBox_TeleportClass.Items.Refresh();
        ListBox_TeleportInfo.Items.Refresh();
    }

    private void ListBox_TeleportClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ListBox_TeleportClass.SelectedIndex;
        if (index != -1)
        {
            ListBox_TeleportInfo.Items.Clear();

            foreach (var item in TeleportData.TeleportDataClass[index].TeleportInfo)
            {
                ListBox_TeleportInfo.Items.Add(item.Name);
            }

            ListBox_TeleportInfo.SelectedIndex = 0;
        }
    }

    private void ListBox_TeleportInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index1 = ListBox_TeleportClass.SelectedIndex;
        var index2 = ListBox_TeleportInfo.SelectedIndex;
        if (index1 != -1 && index2 != -1)
        {
            TempData.TCode = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position;

            if (index1 == 0)
            {
                TextBox_Position_X.IsEnabled = true;
                TextBox_Position_Y.IsEnabled = true;
                TextBox_Position_Z.IsEnabled = true;

                TextBox_Position_Name.IsEnabled = true;

                TextBox_Position_Name.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Name;
                TextBox_Position_X.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position.X.ToString();
                TextBox_Position_Y.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position.Y.ToString();
                TextBox_Position_Z.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position.Z.ToString();
            }
            else
            {
                TextBox_Position_X.IsEnabled = false;
                TextBox_Position_Y.IsEnabled = false;
                TextBox_Position_Z.IsEnabled = false;

                TextBox_Position_Name.IsEnabled = false;

                TextBox_Position_Name.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Name;
                TextBox_Position_X.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position.X.ToString();
                TextBox_Position_Y.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position.Y.ToString();
                TextBox_Position_Z.Text = TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position.Z.ToString();
            }
        }
    }

    private void Button_Teleport_AddCustom_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Vector3 vector3 = Teleport.GetPlayerPosition();

        TeleportData.CustomTeleport.Add(new TeleportData.TeleportInfo()
        {
            Name = $"保存点 : {DateTime.Now:yyyyMMdd_HH-mm-ss_ffff}",
            Position = vector3
        });

        UpdateTeleportList();

        ListBox_TeleportClass.SelectedIndex = 0;
        ListBox_TeleportInfo.SelectedIndex = ListBox_TeleportInfo.Items.Count - 1;
    }

    private void Button_Teleport_EditCustom_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        int index1 = ListBox_TeleportClass.SelectedIndex;
        int index2 = ListBox_TeleportInfo.SelectedIndex;
        if (index1 == 0 && index2 != -1)
        {
            TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Position = new Vector3()
            {
                X = Convert.ToSingle(TextBox_Position_X.Text),
                Y = Convert.ToSingle(TextBox_Position_Y.Text),
                Z = Convert.ToSingle(TextBox_Position_Z.Text)
            };

            TeleportData.TeleportDataClass[index1].TeleportInfo[index2].Name = TextBox_Position_Name.Text;

            UpdateTeleportList();

            ListBox_TeleportClass.SelectedIndex = 0;
            ListBox_TeleportInfo.SelectedIndex = index2; ;

            NotifierHelper.Show(NotifierType.Success, "修改自定义传送坐标成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "当前选中项为空");
        }
    }

    private void Button_Teleport_DeleteCustom_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        int index1 = ListBox_TeleportClass.SelectedIndex;
        int index2 = ListBox_TeleportInfo.SelectedIndex;
        if (index1 == 0 && index2 != -1)
        {
            TeleportData.TeleportDataClass[index1].TeleportInfo.Remove(TeleportData.TeleportDataClass[index1].TeleportInfo[index2]);

            UpdateTeleportList();

            ListBox_TeleportClass.SelectedIndex = 0;
            ListBox_TeleportInfo.SelectedIndex = ListBox_TeleportInfo.Items.Count - 1;

            NotifierHelper.Show(NotifierType.Success, "删除自定义传送坐标成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "当前选中项为空");
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

        Teleport.SetTeleportPosition(TempData.TCode);
    }

    private void Button_Teleport_SaveCustom_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        SaveConfig();

        NotifierHelper.Show(NotifierType.Success, $"保存到自定义传送坐标文件成功");
    }
    #endregion
}
