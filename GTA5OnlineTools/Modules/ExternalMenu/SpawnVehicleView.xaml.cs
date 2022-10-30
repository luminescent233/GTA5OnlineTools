using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Data;
using GTA5OnlineTools.Features.Client;
using GTA5OnlineTools.Features.Settings;

namespace GTA5OnlineTools.Modules.ExternalMenu;

/// <summary>
/// SpawnVehicleView.xaml 的交互逻辑
/// </summary>
public partial class SpawnVehicleView : UserControl
{
    private VehicleSpawn vehicleSpawn = new();

    private List<PerVehInfo> perVehInfos = new();

    public SpawnVehicleView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;

        // 载具列表
        foreach (var item in VehicleData.VehicleClassData)
        {
            ComboBox_VehicleClass.Items.Add(new EmojiMenu()
            {
                Emoji = item.Emoji,
                Title = item.Name
            });
        }
        ComboBox_VehicleClass.SelectedIndex = 0;

        // 载具附加功能
        foreach (var item in MiscData.VehicleExtras)
        {
            ComboBox_VehicleExtras.Items.Add(item.Name);
        }
        ComboBox_VehicleExtras.SelectedIndex = 0;
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {

    }

    private void ComboBox_VehicleClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lock (this)
        {
            var index = ComboBox_VehicleClass.SelectedIndex;
            if (index != -1)
            {
                ListBox_VehicleInfo.Items.Clear();

                Task.Run(() =>
                {
                    var className = VehicleData.VehicleClassData[index].Name;

                    for (int i = 0; i < VehicleData.VehicleClassData[index].VehicleInfo.Count; i++)
                    {
                        var name = VehicleData.VehicleClassData[index].VehicleInfo[i].Name;
                        var displayName = VehicleData.VehicleClassData[index].VehicleInfo[i].DisplayName;

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
                        {
                            if (index == ComboBox_VehicleClass.SelectedIndex)
                            {
                                ListBox_VehicleInfo.Items.Add(new ModelPreview()
                                {
                                    Id = name,
                                    Name = displayName,
                                    Image = $"\\Assets\\Images\\Client\\Vehicles\\{name}.png"
                                });
                            }
                        });
                    }
                });

                ListBox_VehicleInfo.SelectedIndex = 0;
            }
        }
    }

    private void ListBox_VehicleInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        vehicleSpawn.VehicleHash = 0;

        var index1 = ComboBox_VehicleClass.SelectedIndex;
        var index2 = ListBox_VehicleInfo.SelectedIndex;
        if (index1 != -1 && index2 != -1)
        {
            vehicleSpawn.VehicleHash = VehicleData.VehicleClassData[index1].VehicleInfo[index2].Hash;
            vehicleSpawn.VehicleMod = VehicleData.VehicleClassData[index1].VehicleInfo[index2].Mod;
        }
    }

    private void Button_SpawnOnlineVehicleA_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Vehicle.SpawnVehicle(vehicleSpawn.VehicleHash, -255.0f, 5, vehicleSpawn.VehicleMod);
        //Vehicle.SpawnVehicle(vehicleSpawn.VehicleHash, -255.0f);
    }

    private void Button_SpawnOnlineVehicleB_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Vehicle.SpawnVehicle(vehicleSpawn.VehicleHash, 0.0f, 5, vehicleSpawn.VehicleMod);
        //Vehicle.SpawnVehicle(vehicleSpawn.VehicleHash, -255.0f);
    }

    /////////////////////////////////////////////////////////////////////////////////

    private void CheckBox_VehicleGodMode_Click(object sender, RoutedEventArgs e)
    {
        Vehicle.GodMode(CheckBox_VehicleGodMode.IsChecked == true);
        MenuSetting.Vehicle.GodMode = CheckBox_VehicleGodMode.IsChecked == true;
    }

    private void CheckBox_VehicleSeatbelt_Click(object sender, RoutedEventArgs e)
    {
        Vehicle.Seatbelt(CheckBox_VehicleSeatbelt.IsChecked == true);
        MenuSetting.Vehicle.Seatbelt = CheckBox_VehicleSeatbelt.IsChecked == true;
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

    private void Button_GetInOnlinePV_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Online.GetInOnlinePV();
    }

    private void Button_RefushPersonalVehicleList_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        ListBox_PersonalVehicle.Items.Clear();
        perVehInfos.Clear();

        Task.Run(() =>
        {
            int max_slots = Hacks.ReadGA<int>(1585857);
            for (int i = 0; i < max_slots; i++)
            {
                long hash = Hacks.ReadGA<long>(1585857 + 1 + (i * 142) + 66);
                if (hash == 0)
                    continue;

                string plate = Hacks.ReadGAString(1585857 + 1 + (i * 142) + 1);

                perVehInfos.Add(new PerVehInfo()
                {
                    Index = i,
                    Name = Vehicle.FindVehicleDisplayName(hash, true),
                    Hash = hash,
                    Plate = plate
                });
            }

            foreach (var item in perVehInfos)
            {
                this.Dispatcher.Invoke(() =>
                {
                    ListBox_PersonalVehicle.Items.Add($"[{item.Plate}]\t{item.Name}");
                });
            }
        });
    }

    private void Button_SpawnPersonalVehicle_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        var index = ListBox_PersonalVehicle.SelectedIndex;
        if (index != -1)
        {
            Task.Run(() =>
            {
                Vehicle.SpawnPersonalVehicle(perVehInfos[index].Index);
            });
        }
    }

    private void ComboBox_VehicleExtras_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ComboBox_VehicleExtras.SelectedIndex;
        if (index != -1)
        {
            Vehicle.Extras((short)MiscData.VehicleExtras[index].ID);
        }
    }
}
