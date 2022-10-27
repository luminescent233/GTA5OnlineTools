using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Data;

namespace GTA5OnlineTools.Modules.ExternalMenu;

/// <summary>
/// SpawnVehicleView.xaml 的交互逻辑
/// </summary>
public partial class SpawnVehicleView : UserControl
{
    private long SpawnVehicleHash = 0;
    private int[] SpawnVehicleMod;

    private List<PVInfo> pVInfos = new();

    public SpawnVehicleView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;

        // 载具列表
        for (int i = 0; i < VehicleData.VehicleClassData.Count; i++)
        {
            ListBox_VehicleClass.Items.Add(VehicleData.VehicleClassData[i].ClassName);
        }
        ListBox_VehicleClass.SelectedIndex = 0;
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {

    }

    private void ListBox_VehicleClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int index = ListBox_VehicleClass.SelectedIndex;

        if (index != -1)
        {
            ListBox_VehicleInfo.Items.Clear();

            for (int i = 0; i < VehicleData.VehicleClassData[index].VehicleInfo.Count; i++)
            {
                ListBox_VehicleInfo.Items.Add(VehicleData.VehicleClassData[index].VehicleInfo[i].DisplayName);
            }

            ListBox_VehicleInfo.SelectedIndex = 0;
        }
    }

    private void ListBox_VehicleInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SpawnVehicleHash = 0;

        int index1 = ListBox_VehicleClass.SelectedIndex;
        int index2 = ListBox_VehicleInfo.SelectedIndex;

        if (index1 != -1 && index2 != -1)
        {
            SpawnVehicleHash = VehicleData.VehicleClassData[index1].VehicleInfo[index2].Hash;
            SpawnVehicleMod = VehicleData.VehicleClassData[index1].VehicleInfo[index2].Mod;
        }
    }

    private void Button_SpawnOnlineVehicle_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        string str = (e.OriginalSource as Button).Content.ToString();

        if (str == "刷出线上载具（空地）")
        {
            Vehicle.SpawnVehicle(SpawnVehicleHash, -255.0f, 5, SpawnVehicleMod);
            //Vehicle.SpawnVehicle(SpawnVehicleHash, -255.0f);
        }
        else
        {
            Vehicle.SpawnVehicle(SpawnVehicleHash, 0.0f, 5, SpawnVehicleMod);
            //Vehicle.SpawnVehicle(SpawnVehicleHash, -255.0f);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    private void CheckBox_VehicleGodMode_Click(object sender, RoutedEventArgs e)
    {
        Vehicle.GodMode(Settings.Vehicle.VehicleGodMode = true);
        Settings.Vehicle.VehicleGodMode = CheckBox_VehicleGodMode.IsChecked == true;
    }

    private void CheckBox_VehicleSeatbelt_Click(object sender, RoutedEventArgs e)
    {
        Vehicle.Seatbelt(CheckBox_VehicleSeatbelt.IsChecked == true);
        Settings.Vehicle.VehicleSeatbelt = CheckBox_VehicleSeatbelt.IsChecked == true;
    }

    private void CheckBox_VehicleParachute_Click(object sender, RoutedEventArgs e)
    {
        Vehicle.Parachute(CheckBox_VehicleParachute.IsChecked == true);
    }

    private void CheckBox_VehicleInvisibility_Click(object sender, RoutedEventArgs e)
    {
        Vehicle.Invisible(CheckBox_VehicleInvisibility.IsChecked == true);
    }

    private void Button_FillVehicleHealth_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Vehicle.FillHealth();
    }

    private void RadioButton_VehicleExtras_None_Click(object sender, RoutedEventArgs e)
    {
        if (RadioButton_VehicleExtras_None.IsChecked == true)
        {
            Vehicle.Extras(0);
        }
        else if (RadioButton_VehicleExtras_Jump.IsChecked == true)
        {
            Vehicle.Extras(40);
        }
        else if (RadioButton_VehicleExtras_Boost.IsChecked == true)
        {
            Vehicle.Extras(66);
        }
        else if (RadioButton_VehicleExtras_Both.IsChecked == true)
        {
            Vehicle.Extras(96);
        }
    }

    private void Button_RepairVehicle_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Vehicle.Fix1stfoundBST();
    }

    private void Button_TurnOffBST_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Online.InstantBullShark(false);
    }

    private void Button_GetInOnlinePV_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Online.GetInOnlinePV();
    }

    private void Button_RefushPersonalVehicleList_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        ListBox_PersonalVehicle.Items.Clear();
        pVInfos.Clear();

        Task.Run(() =>
        {
            int max_slots = Hacks.ReadGA<int>(1585857);
            for (int i = 0; i < max_slots; i++)
            {
                long hash = Hacks.ReadGA<long>(1585857 + 1 + (i * 142) + 66);
                if (hash == 0)
                    continue;

                string plate = Hacks.ReadGAString(1585857 + 1 + (i * 142) + 1);

                pVInfos.Add(new PVInfo()
                {
                    Index = i,
                    Name = Vehicle.FindVehicleDisplayName(hash, true),
                    hash = hash,
                    plate = plate
                });
            }

            foreach (var item in pVInfos)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ListBox_PersonalVehicle.Items.Add($"[{item.plate}]\t{item.Name}");
                });
            }
        });
    }

    private void Button_SpawnPersonalVehicle_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        int index = ListBox_PersonalVehicle.SelectedIndex;
        if (index != -1)
        {
            Task.Run(() =>
            {
                Vehicle.SpawnPersonalVehicle(pVInfos[index].Index);
            });
        }
    }
}

public struct PVInfo
{
    public int Index;
    public string Name;
    public long hash;
    public string plate;
}
