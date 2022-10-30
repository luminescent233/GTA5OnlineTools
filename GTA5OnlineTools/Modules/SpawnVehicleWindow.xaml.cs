using GTA5OnlineTools.Features.Client;

namespace GTA5OnlineTools.Modules;

/// <summary>
/// SpawnVehicleWindow.xaml 的交互逻辑
/// </summary>
public partial class SpawnVehicleWindow
{
    public SpawnVehicleWindow()
    {
        InitializeComponent();
    }

    private void Window_SpawnVehicle_Loaded(object sender, RoutedEventArgs e)
    {
        // 载具列表
        foreach (var item in VehicleData.VehicleClassData)
        {
            ListBox_VehicleClass.Items.Add(item.ClassName);
        }
        ListBox_VehicleClass.SelectedIndex = 0;
    }

    private void Window_SpawnVehicle_Closing(object sender, CancelEventArgs e)
    {

    }

    private void ListBox_VehicleClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lock (this)
        {
            var index = ListBox_VehicleClass.SelectedIndex;
            if (index != -1)
            {
                ListBox_VehicleInfo.Items.Clear();

                Task.Run(() =>
                {
                    var className = VehicleData.VehicleClassData[index].ClassName;

                    for (int i = 0; i < VehicleData.VehicleClassData[index].VehicleInfo.Count; i++)
                    {
                        var name = VehicleData.VehicleClassData[index].VehicleInfo[i].Name;
                        var displayName = VehicleData.VehicleClassData[index].VehicleInfo[i].DisplayName;

                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
                        {
                            if (index == ListBox_VehicleClass.SelectedIndex)
                            {
                                ListBox_VehicleInfo.Items.Add(new VehiclePreview()
                                {
                                    VehicleId = name,
                                    VehicleName = displayName,
                                    VehicleImage = $"\\Assets\\Images\\Client\\Vehicles\\{name}.png"
                                });
                            }
                        });
                    }
                });

                ListBox_VehicleInfo.SelectedIndex = 0;
            }
        }
    }
}

public class VehiclePreview
{
    public string VehicleId { get; set; }
    public string VehicleName { get; set; }
    public string VehicleImage { get; set; }
}
