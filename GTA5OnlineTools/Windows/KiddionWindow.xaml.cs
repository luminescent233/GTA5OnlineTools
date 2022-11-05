using GTA5OnlineTools.Models;
using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Common.Helper;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Common.API;

namespace GTA5OnlineTools.Windows;

/// <summary>
/// KiddionWindow.xaml 的交互逻辑
/// </summary>
public partial class KiddionWindow
{
    public ObservableCollection<KiddionModel> KiddionModels { get; set; } = new();

    public KiddionWindow()
    {
        InitializeComponent();
    }

    private void Window_Kiddion_Loaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = this;
    }

    private void Window_Kiddion_Closing(object sender, CancelEventArgs e)
    {

    }

    private void Button_GetKiddionUI_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        lock (this)
        {
            KiddionModels.Clear();

            var pArray = Process.GetProcessesByName("Kiddion");
            if (pArray.Length > 0)
            {
                var main_handle = pArray[0].MainWindowHandle;

                int index = 0;

                var button_handle = Win32.FindWindowEx(main_handle, IntPtr.Zero, "Button", null);
                while (button_handle != IntPtr.Zero)
                {
                    var length = Win32.GetWindowTextLength(button_handle);
                    var builder = new StringBuilder(length + 1);
                    Win32.GetWindowText(button_handle, builder, builder.Capacity);

                    var text = builder.ToString();
                    var split = text.IndexOf("|");
                    if (split != -1)
                        text = text[..split];
                    KiddionModels.Add(new KiddionModel()
                    {
                        Index = ++index,
                        Name = text,
                        Value = "中文翻译写这里"
                    });

                    button_handle = Win32.FindWindowEx(main_handle, button_handle, "Button", null);
                }
            }
            else
            {
                NotifierHelper.Show(NotifierType.Warning, "未发现《Kiddion》进程，请先运行《Kiddion》程序");
            }
        }
    }

    private void Button_TranslateSelected_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        var value = TextBox_SelectedItem_Vaule.Text.Trim();
        if (!string.IsNullOrEmpty(value))
        {
            var index = DataGrid_KiddionUIs.SelectedIndex;

            KiddionModels[index].Value = value;
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "翻译内容为空，操作取消");
        }
    }

    private async void Button_YouDaoTranslateSelected_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        var name = TextBox_SelectedItem_Name.Text.Trim();
        if (!string.IsNullOrEmpty(name))
        {
            var index = DataGrid_KiddionUIs.SelectedIndex;

            KiddionModels[index].Value = await WebAPI.GetYouDaoContent(name);
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "翻译内容为空，操作取消");
        }
    }

    private void Button_TranslateBuild_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        TextBox_TranslateBuild.Clear();

        foreach (var item in KiddionModels)
        {
            TextBox_TranslateBuild.AppendText($"\t{{ L\"{item.Name}\", L\"{item.Value}\" }},\n");
        }
    }
}
