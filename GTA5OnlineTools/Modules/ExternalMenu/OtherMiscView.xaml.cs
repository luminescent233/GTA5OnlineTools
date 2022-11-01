using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Client;

namespace GTA5OnlineTools.Modules.ExternalMenu;

/// <summary>
/// OtherMiscView.xaml 的交互逻辑
/// </summary>
public partial class OtherMiscView : UserControl
{
    public OtherMiscView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;

        // Ped列表
        foreach (var item in PedData.PedDataClass)
        {
            ListBox_PedModel.Items.Add(item.DisplayName);
        }
        ListBox_PedModel.SelectedIndex = 0;
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {
        
    }

    private void Button_ModelChange_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        var index = ListBox_PedModel.SelectedIndex;
        if (index != -1)
            Online.ModelChange(PedData.PedDataClass[index].Hash);
    }
}
