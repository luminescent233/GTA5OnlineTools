using GTA5OnlineTools.Features;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Data;
using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Config.Modules;
using GTA5OnlineTools.Models.Modules;

namespace GTA5OnlineTools.Modules.ExternalMenu;

/// <summary>
/// SelfStateView.xaml 的交互逻辑
/// </summary>
public partial class SelfStateView : UserControl
{
    /// <summary>
    /// SelfState 数据模型
    /// </summary>
    public SelfStateModel SelfStateModel { get; set; } = new();

    /// <summary>
    /// Option配置文件集，以json格式保存到本地
    /// </summary>
    private SelfStateConfig SelfStateConfig { get; set; } = new();

    /// <summary>
    /// 角色无碰撞体积切换
    /// </summary>
    private bool _NoCollisionToggle = false;

    public SelfStateView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;

        new Thread(SelfStateMainThread)
        {
            Name = "SelfStateMainThread",
            IsBackground = true
        }.Start();

        new Thread(SelfStateCommonThread)
        {
            Name = "SelfStateCommonThread",
            IsBackground = true
        }.Start();

        HotKeys.AddKey(WinVK.F3);
        HotKeys.AddKey(WinVK.F4);
        HotKeys.AddKey(WinVK.F5);
        HotKeys.AddKey(WinVK.F6);
        HotKeys.AddKey(WinVK.F7);
        HotKeys.AddKey(WinVK.F8);
        HotKeys.AddKey(WinVK.Oem0);
        HotKeys.KeyDownEvent += HotKeys_KeyDownEvent;

        // 如果配置文件不存在就创建
        if (!File.Exists(FileUtil.F_SelfStateConfig_Path))
        {
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(FileUtil.F_SelfStateConfig_Path))
        {
            using var streamReader = new StreamReader(FileUtil.F_SelfStateConfig_Path);
            SelfStateConfig = JsonUtil.JsonDese<SelfStateConfig>(streamReader.ReadToEnd());

            SelfStateModel.IsHotKeyToWaypoint = SelfStateConfig.IsHotKeyToWaypoint;
            SelfStateModel.IsHotKeyToObjective = SelfStateConfig.IsHotKeyToObjective;
            SelfStateModel.IsHotKeyFillHealthArmor = SelfStateConfig.IsHotKeyFillHealthArmor;
            SelfStateModel.IsHotKeyClearWanted = SelfStateConfig.IsHotKeyClearWanted;

            SelfStateModel.IsHotKeyFillAllAmmo = SelfStateConfig.IsHotKeyFillAllAmmo;
            SelfStateModel.IsHotKeyMovingFoward = SelfStateConfig.IsHotKeyMovingFoward;

            SelfStateModel.IsHotKeyNoCollision = SelfStateConfig.IsHotKeyNoCollision;
        }
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
        SelfStateConfig.IsHotKeyToWaypoint = SelfStateModel.IsHotKeyToWaypoint;
        SelfStateConfig.IsHotKeyToObjective = SelfStateModel.IsHotKeyToObjective;
        SelfStateConfig.IsHotKeyFillHealthArmor = SelfStateModel.IsHotKeyFillHealthArmor;
        SelfStateConfig.IsHotKeyClearWanted = SelfStateModel.IsHotKeyClearWanted;

        SelfStateConfig.IsHotKeyFillAllAmmo = SelfStateModel.IsHotKeyFillAllAmmo;
        SelfStateConfig.IsHotKeyMovingFoward = SelfStateModel.IsHotKeyMovingFoward;

        SelfStateConfig.IsHotKeyNoCollision = SelfStateModel.IsHotKeyNoCollision;

        // 写入到Json文件
        File.WriteAllText(FileUtil.F_SelfStateConfig_Path, JsonUtil.JsonSeri(SelfStateConfig));
    }

    /// <summary>
    /// 全局热键 按键按下事件
    /// </summary>
    /// <param name="vK"></param>
    private void HotKeys_KeyDownEvent(WinVK vK)
    {
        switch (vK)
        {
            case WinVK.F3:
                if (SelfStateModel.IsHotKeyFillAllAmmo)
                {
                    Weapon.FillAllAmmo();
                }
                break;
            case WinVK.F4:
                if (SelfStateModel.IsHotKeyMovingFoward)
                {
                    Teleport.MovingFoward();
                }
                break;
            case WinVK.F5:
                if (SelfStateModel.IsHotKeyToWaypoint)
                {
                    Teleport.ToWaypoint();
                }
                break;
            case WinVK.F6:
                if (SelfStateModel.IsHotKeyToObjective)
                {
                    Teleport.ToObjective();
                }
                break;
            case WinVK.F7:
                if (SelfStateModel.IsHotKeyFillHealthArmor)
                {
                    Player.FillHealthArmor();
                }
                break;
            case WinVK.F8:
                if (SelfStateModel.IsHotKeyClearWanted)
                {
                    Player.WantedLevel(0x00);
                }
                break;
            case WinVK.Oem0:
                if (SelfStateModel.IsHotKeyNoCollision)
                {
                    _NoCollisionToggle = !_NoCollisionToggle;

                    Player.NoCollision(_NoCollisionToggle);
                    Settings.Player.NoCollision = _NoCollisionToggle;

                    if (_NoCollisionToggle)
                        Console.Beep(600, 75);
                    else
                        Console.Beep(500, 75);
                }
                break;
        }
    }

    private void SelfStateMainThread()
    {
        while (Globals.IsAppRunning)
        {
            float oHealth = GTA5Mem.Read<float>(General.WorldPTR, Offsets.Player.Health);
            float oMaxHealth = GTA5Mem.Read<float>(General.WorldPTR, Offsets.Player.MaxHealth);
            float oArmor = GTA5Mem.Read<float>(General.WorldPTR, Offsets.Player.Armor);

            byte oWanted = GTA5Mem.Read<byte>(General.WorldPTR, Offsets.Player.Wanted);
            float oRunSpeed = GTA5Mem.Read<float>(General.WorldPTR, Offsets.Player.RunSpeed);
            float oSwimSpeed = GTA5Mem.Read<float>(General.WorldPTR, Offsets.Player.SwimSpeed);
            float oStealthSpeed = GTA5Mem.Read<float>(General.WorldPTR, Offsets.Player.StealthSpeed);

            byte oInVehicle = GTA5Mem.Read<byte>(General.WorldPTR, Offsets.InVehicle);
            byte oCurPassenger = GTA5Mem.Read<byte>(General.WorldPTR, Offsets.Vehicle.CurPassenger);

            ////////////////////////////////

            if (Settings.Player.GodMode)
                Player.GodMode(true);

            if (Settings.Player.AntiAFK)
                Player.AntiAFK(true);

            if (Settings.Player.NoRagdoll)
                Player.NoRagdoll(true);

            if (Settings.Player.NoCollision)
                GTA5Mem.Write(General.WorldPTR, Offsets.Player.NoCollision, -1.0f);

            if (Settings.Vehicle.VehicleGodMode)
                GTA5Mem.Write<byte>(General.WorldPTR, Offsets.Vehicle.GodMode, 0x01);

            if (Settings.Vehicle.VehicleSeatbelt)
                GTA5Mem.Write<byte>(General.WorldPTR, Offsets.Player.Seatbelt, 0xC9);

            ////////////////////////////////

            this.Dispatcher.BeginInvoke(() =>
            {
                if (Slider_Health.Value != oHealth)
                    Slider_Health.Value = oHealth;

                if (Slider_MaxHealth.Value != oMaxHealth)
                    Slider_MaxHealth.Value = oMaxHealth;

                if (Slider_Armor.Value != oArmor)
                    Slider_Armor.Value = oArmor;

                if (Slider_Wanted.Value != oWanted)
                    Slider_Wanted.Value = oWanted;

                if (Slider_RunSpeed.Value != oRunSpeed)
                    Slider_RunSpeed.Value = oRunSpeed;

                if (Slider_SwimSpeed.Value != oSwimSpeed)
                    Slider_SwimSpeed.Value = oSwimSpeed;

                if (Slider_StealthSpeed.Value != oStealthSpeed)
                    Slider_StealthSpeed.Value = oStealthSpeed;
            });

            Thread.Sleep(1000);
        }
    }

    private void SelfStateCommonThread()
    {
        while (Globals.IsAppRunning)
        {
            if (Settings.Common.AutoClearWanted)
                Player.WantedLevel(0x00);

            if (Settings.Common.AutoKillNPC)
                World.KillNPC(false);

            if (Settings.Common.AutoKillHostilityNPC)
                World.KillNPC(true);

            if (Settings.Common.AutoKillPolice)
                World.KillPolice();

            Thread.Sleep(200);
        }
    }

    private void Slider_Health_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Player.Health, (float)Slider_Health.Value);
    }

    private void Slider_MaxHealth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Player.MaxHealth, (float)Slider_MaxHealth.Value);
    }

    private void Slider_Armor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Player.Armor, (float)Slider_Armor.Value);
    }

    private void Slider_Wanted_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<byte>(General.WorldPTR, Offsets.Player.Wanted, (byte)Slider_Wanted.Value);
    }

    private void Slider_RunSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Player.RunSpeed, (float)Slider_RunSpeed.Value);
    }

    private void Slider_SwimSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Player.SwimSpeed, (float)Slider_SwimSpeed.Value);
    }

    private void Slider_StealthSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Player.StealthSpeed, (float)Slider_StealthSpeed.Value);
    }

    private void CheckBox_PlayerGodMode_Click(object sender, RoutedEventArgs e)
    {
        Player.GodMode(CheckBox_PlayerGodMode.IsChecked == true);
        Settings.Player.GodMode = CheckBox_PlayerGodMode.IsChecked == true;
    }

    private void CheckBox_AntiAFK_Click(object sender, RoutedEventArgs e)
    {
        Player.AntiAFK(CheckBox_AntiAFK.IsChecked == true);
        Settings.Player.AntiAFK = CheckBox_AntiAFK.IsChecked == true;
    }

    private void CheckBox_Invisibility_Click(object sender, RoutedEventArgs e)
    {
        Player.Invisibility(CheckBox_Invisibility.IsChecked == true);
    }

    private void CheckBox_UndeadOffRadar_Click(object sender, RoutedEventArgs e)
    {
        Player.UndeadOffRadar(CheckBox_UndeadOffRadar.IsChecked == true);
    }

    private void CheckBox_NoRagdoll_Click(object sender, RoutedEventArgs e)
    {
        Player.NoRagdoll(CheckBox_NoRagdoll.IsChecked == true);
        Settings.Player.NoRagdoll = CheckBox_NoRagdoll.IsChecked == true;
    }

    private void CheckBox_NPCIgnore_Click(object sender, RoutedEventArgs e)
    {
        if (CheckBox_NPCIgnore.IsChecked == true && CheckBox_PoliceIgnore.IsChecked == false)
        {
            GTA5Mem.Write<byte>(General.WorldPTR, Offsets.NPCIgnore, 0x04);
        }
        else if (CheckBox_NPCIgnore.IsChecked == false && CheckBox_PoliceIgnore.IsChecked == true)
        {
            GTA5Mem.Write<byte>(General.WorldPTR, Offsets.NPCIgnore, 0xC3);
        }
        else if (CheckBox_NPCIgnore.IsChecked == true && CheckBox_PoliceIgnore.IsChecked == true)
        {
            GTA5Mem.Write<byte>(General.WorldPTR, Offsets.NPCIgnore, 0xC7);
        }
        else
        {
            GTA5Mem.Write<byte>(General.WorldPTR, Offsets.NPCIgnore, 0x00);
        }
    }

    private void CheckBox_AutoClearWanted_Click(object sender, RoutedEventArgs e)
    {
        Player.WantedLevel(0x00);
        Settings.Common.AutoClearWanted = CheckBox_AutoClearWanted.IsChecked == true;
    }

    private void CheckBox_AutoKillNPC_Click(object sender, RoutedEventArgs e)
    {
        World.KillNPC(false);
        Settings.Common.AutoKillNPC = CheckBox_AutoKillNPC.IsChecked == true;
    }

    private void CheckBox_AutoKillHostilityNPC_Click(object sender, RoutedEventArgs e)
    {
        World.KillNPC(true);
        Settings.Common.AutoKillHostilityNPC = CheckBox_AutoKillHostilityNPC.IsChecked == true;
    }

    private void CheckBox_AutoKillPolice_Click(object sender, RoutedEventArgs e)
    {
        World.KillPolice();
        Settings.Common.AutoKillPolice = CheckBox_AutoKillPolice.IsChecked == true;
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

    private void Button_FillHealthArmor_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Player.FillHealthArmor();
    }

    private void Button_ClearWanted_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Player.WantedLevel(0x00);
    }

    private void Button_Suicide_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        Player.Suicide();
    }

    private void Slider_MovingFoward_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Settings.Forward = (float)Slider_MovingFoward.Value;
    }

    private void CheckBox_ProofBullet_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofBullet(CheckBox_ProofBullet.IsChecked == true);
    }

    private void CheckBox_ProofFire_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofFire(CheckBox_ProofFire.IsChecked == true);
    }

    private void CheckBox_ProofCollision_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofCollision(CheckBox_ProofCollision.IsChecked == true);
    }

    private void CheckBox_ProofMelee_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofMelee(CheckBox_ProofMelee.IsChecked == true);
    }

    private void CheckBox_ProofExplosion_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofExplosion(CheckBox_ProofExplosion.IsChecked == true);
    }

    private void CheckBox_ProofSteam_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofSteam(CheckBox_ProofSteam.IsChecked == true);
    }

    private void CheckBox_ProofDrown_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofDrown(CheckBox_ProofDrown.IsChecked == true);
    }

    private void CheckBox_ProofWater_Click(object sender, RoutedEventArgs e)
    {
        Player.ProofWater(CheckBox_ProofWater.IsChecked == true);
    }

    private void CheckBox_NoCollision_Click(object sender, RoutedEventArgs e)
    {
        _NoCollisionToggle = SelfStateModel.IsHotKeyNoCollision;

        Player.NoCollision(_NoCollisionToggle);
        Settings.Player.NoCollision = _NoCollisionToggle;
    }
}
