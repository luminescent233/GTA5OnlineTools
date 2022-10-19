using GTA5OnlineTools.Common.Data;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Data;
using GTA5OnlineTools.Modules.ExternalMenu;

using CommunityToolkit.Mvvm.Input;

namespace GTA5OnlineTools.Modules;

/// <summary>
/// ExternalMenuWindow.xaml 的交互逻辑
/// </summary>
public partial class ExternalMenuWindow
{
    /// <summary>
    /// 导航菜单
    /// </summary>
    public List<MenuBar> MenuBars { get; set; } = new();
    /// <summary>
    /// 导航命令
    /// </summary>
    public RelayCommand<MenuBar> NavigateCommand { get; private set; }

    private readonly ReadMeView ReadMeView = new();

    private readonly SelfStateView SelfStateView = new();
    private readonly WorldFunctionView WorldFunctionView = new();
    private readonly OnlineOptionView OnlineOptionView = new();
    private readonly PlayerListView PlayerListView = new();
    private readonly SpawnVehicleView SpawnVehicleView = new();
    private readonly SpawnWeaponView SpawnWeaponView = new();
    private readonly ExternalOverlayView ExternalOverlayView = new();
    private readonly SessionChatView SessionChatView = new();
    private readonly JobHelperView JobHelperView = new();

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// 主窗口关闭委托
    /// </summary>
    public delegate void WindowClosingDelegate();
    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    public static event WindowClosingDelegate WindowClosingEvent;

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// 主窗口 窗口句柄
    /// </summary>
    private IntPtr ThisWinHandle;
    /// <summary>
    /// 主窗口 鼠标坐标数据
    /// </summary>
    private POINT ThisWinPOINT;

    public ExternalMenuWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 外置菜单窗口加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_ExternalMenu_Loaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = this;

        // 创建菜单
        CreateMenuBar();
        // 绑定菜单切换命令
        NavigateCommand = new(Navigate);
        // 设置主页
        ContentControl_Main.Content = SelfStateView;

        ///////////////////////////////////////////////////////////////

        // 获取自身窗口句柄
        ThisWinHandle = new WindowInteropHelper(this).Handle;
        Win32.GetCursorPos(out ThisWinPOINT);

        HotKeys.AddKey(WinVK.DELETE);
        HotKeys.KeyDownEvent += HotKeys_KeyDownEvent;
    }

    /// <summary>
    /// 外置菜单窗口关闭事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_ExternalMenu_Closing(object sender, CancelEventArgs e)
    {
        WindowClosingEvent();
    }

    /// <summary>
    /// 创建导航菜单
    /// </summary>
    private void CreateMenuBar()
    {
        MenuBars.Add(new MenuBar() { Emoji = "🍎", Title = "自身属性", NameSpace = "SelfStateView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍐", Title = "世界功能", NameSpace = "WorldFunctionView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍋", Title = "线上选项", NameSpace = "OnlineOptionView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍉", Title = "玩家列表", NameSpace = "PlayerListView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍇", Title = "线上载具", NameSpace = "SpawnVehicleView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍓", Title = "线上武器", NameSpace = "SpawnWeaponView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍈", Title = "外部ESP", NameSpace = "ExternalOverlayView" });
        MenuBars.Add(new MenuBar() { Emoji = "🍑", Title = "战局聊天", NameSpace = "SessionChatView" });
        MenuBars.Add(new MenuBar() { Emoji = "🥭", Title = "任务帮手", NameSpace = "JobHelperView" });

        MenuBars.Add(new MenuBar() { Emoji = "💌", Title = "README", NameSpace = "ReadMeView" });
    }

    /// <summary>
    /// 页面导航（重复点击不会重复触发）
    /// </summary>
    /// <param name="menu"></param>
    private void Navigate(MenuBar menu)
    {
        if (menu == null || string.IsNullOrEmpty(menu.NameSpace))
            return;

        switch (menu.NameSpace)
        {
            case "ReadMeView":
                ContentControl_Main.Content = ReadMeView;
                break;
            case "SelfStateView":
                ContentControl_Main.Content = SelfStateView;
                break;
            case "WorldFunctionView":
                ContentControl_Main.Content = WorldFunctionView;
                break;
            case "OnlineOptionView":
                ContentControl_Main.Content = OnlineOptionView;
                break;
            case "PlayerListView":
                ContentControl_Main.Content = PlayerListView;
                break;
            case "SpawnVehicleView":
                ContentControl_Main.Content = SpawnVehicleView;
                break;
            case "SpawnWeaponView":
                ContentControl_Main.Content = SpawnWeaponView;
                break;
            case "ExternalOverlayView":
                ContentControl_Main.Content = ExternalOverlayView;
                break;
            case "SessionChatView":
                ContentControl_Main.Content = SessionChatView;
                break;
            case "JobHelperView":
                ContentControl_Main.Content = JobHelperView;
                break;
        }
    }

    /// <summary>
    /// 外置菜单窗口是否置顶
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckBox_IsTopMost_Click(object sender, RoutedEventArgs e)
    {
        if (CheckBox_IsTopMost.IsChecked == true)
            Topmost = true;
        else
            Topmost = false;
    }

    /// <summary>
    /// 全局热键 按键按下事件
    /// </summary>
    /// <param name="vK"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void HotKeys_KeyDownEvent(WinVK vK)
    {
        this.Dispatcher.BeginInvoke(() =>
        {
            switch (vK)
            {
                case WinVK.DELETE:
                    ShowWindow();
                    break;
            }
        });
    }

    /// <summary>
    /// 显示隐藏外置菜单窗口
    /// </summary>
    private void ShowWindow()
    {
        Settings.ShowWindow = !Settings.ShowWindow;
        if (Settings.ShowWindow)
        {
            //Show();
            WindowState = WindowState.Normal;
            this.Focus();

            if (CheckBox_IsTopMost.IsChecked == false)
            {
                Topmost = true;
                Topmost = false;
            }

            Win32.SetCursorPos(ThisWinPOINT.X, ThisWinPOINT.Y);
            Win32.SetForegroundWindow(ThisWinHandle);
        }
        else
        {
            //Hide();
            WindowState = WindowState.Minimized;

            Win32.GetCursorPos(out ThisWinPOINT);
            GTA5Mem.SetForegroundWindow();
        }
    }
}
