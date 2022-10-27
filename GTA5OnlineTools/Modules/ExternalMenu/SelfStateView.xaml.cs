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
            long pCPedFactory = Memory.Read<long>(General.WorldPTR);
            long pCPed = Memory.Read<long>(pCPedFactory + Offsets.CPed);
            long pCPlayerInfo = Memory.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);
            long pCNavigation = Memory.Read<long>(pCPed + Offsets.CPed_CNavigation);

            byte oInVehicle = Memory.Read<byte>(pCPed + Offsets.CPed_InVehicle);

            byte oGod = Memory.Read<byte>(pCPed + Offsets.CPed_God);
            float oHealth = Memory.Read<float>(pCPed + Offsets.CPed_Health);
            float oHealthMax = Memory.Read<float>(pCPed + Offsets.CPed_HealthMax);
            float oArmor = Memory.Read<float>(pCPed + Offsets.CPed_Armor);
            byte oRagdoll = Memory.Read<byte>(pCPed + Offsets.CPed_Ragdoll);
            byte oSeatbelt = Memory.Read<byte>(pCPed + Offsets.CPed_Seatbelt);

            byte oWantedLevel = Memory.Read<byte>(pCPlayerInfo + Offsets.CPed_CPlayerInfo_WantedLevel);
            float oRunSpeed = Memory.Read<float>(pCPlayerInfo + Offsets.CPed_CPlayerInfo_RunSpeed);
            float oSwimSpeed = Memory.Read<float>(pCPlayerInfo + Offsets.CPed_CPlayerInfo_SwimSpeed);
            float oWalkSpeed = Memory.Read<float>(pCPlayerInfo + Offsets.CPed_CPlayerInfo_WalkSpeed);

            ////////////////////////////////////////////////////////////////

            // 玩家无敌
            if (Settings.Player.GodMode)
            {
                if (oGod == 0x00)
                    Memory.Write<byte>(pCPed + Offsets.CPed_God, 0x01);
            }
            else
            {
                if (oGod == 0x01)
                    Memory.Write<byte>(pCPed + Offsets.CPed_God, 0x00);
            }

            // 挂机防踢
            if (Settings.Player.AntiAFK)
            {
                if (Hacks.ReadGA<int>(262145 + 87) == 120000)
                    Player.AntiAFK(true);
            }
            else
            {
                if (Hacks.ReadGA<int>(262145 + 87) == 99999999)
                    Player.AntiAFK(false);
            }

            // 无布娃娃
            if (Settings.Player.NoRagdoll)
            {
                if (oRagdoll == 0x20)
                    Memory.Write<byte>(pCPed + Offsets.CPed_Ragdoll, 0x01);
            }
            else
            {
                if (oRagdoll == 0x01)
                    Memory.Write<byte>(pCPed + Offsets.CPed_Ragdoll, 0x20);
            }

            // 玩家无碰撞体积
            if (Settings.Player.NoCollision)
            {
                long pointer = Memory.Read<long>(pCNavigation + 0x10);
                pointer = Memory.Read<long>(pointer + 0x20);
                pointer = Memory.Read<long>(pointer + 0x70);
                pointer = Memory.Read<long>(pointer + 0x00);
                Memory.Write(pointer + 0x2C, -1.0f);
            }

            // 安全带
            if (Settings.Vehicle.VehicleSeatbelt)
            {
                if (oSeatbelt == 0xC8)
                    Memory.Write<byte>(pCPed + Offsets.CPed_Seatbelt, 0xC9);
            }
            else
            {
                if (oSeatbelt == 0xC9)
                    Memory.Write<byte>(pCPed + Offsets.CPed_Seatbelt, 0xC8);
            }

            ////////////////////////////////////////////////////////////////

            if (oInVehicle != 0x00)
            {
                long pCVehicle = Memory.Read<long>(pCPed + Offsets.CPed_CVehicle);
                byte oVehicleGod = Memory.Read<byte>(pCVehicle + Offsets.CPed_CVehicle_God);

                // 载具无敌
                if (Settings.Vehicle.VehicleGodMode)
                {
                    if (oVehicleGod == 0x00)
                        Memory.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_God, 0x01);
                }
                else
                {
                    if (oVehicleGod == 0x01)
                        Memory.Write<byte>(pCVehicle + Offsets.CPed_CVehicle_God, 0x00);
                }
            }

            ////////////////////////////////////////////////////////////////

            this.Dispatcher.BeginInvoke(() =>
            {
                if (Slider_Health.Value != oHealth)
                    Slider_Health.Value = oHealth;

                if (Slider_HealthMax.Value != oHealthMax)
                    Slider_HealthMax.Value = oHealthMax;

                if (Slider_Armor.Value != oArmor)
                    Slider_Armor.Value = oArmor;

                if (Slider_WantedLevel.Value != oWantedLevel)
                    Slider_WantedLevel.Value = oWantedLevel;

                if (Slider_RunSpeed.Value != oRunSpeed)
                    Slider_RunSpeed.Value = oRunSpeed;

                if (Slider_SwimSpeed.Value != oSwimSpeed)
                    Slider_SwimSpeed.Value = oSwimSpeed;

                if (Slider_WalkSpeed.Value != oWalkSpeed)
                    Slider_WalkSpeed.Value = oWalkSpeed;
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
        Player.Health((float)Slider_Health.Value);
    }

    private void Slider_HealthMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.HealthMax((float)Slider_HealthMax.Value);
    }

    private void Slider_Armor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.Armor((float)Slider_Armor.Value);
    }

    private void Slider_WantedLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.WantedLevel((byte)Slider_WantedLevel.Value);
    }

    private void Slider_RunSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.RunSpeed((float)Slider_RunSpeed.Value);
    }

    private void Slider_SwimSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.SwimSpeed((float)Slider_SwimSpeed.Value);
    }

    private void Slider_WalkSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.WalkSpeed((float)Slider_WalkSpeed.Value);
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
        Player.Invisible(CheckBox_Invisibility.IsChecked == true);
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
            Player.NPCIgnore(0x04);
        }
        else if (CheckBox_NPCIgnore.IsChecked == false && CheckBox_PoliceIgnore.IsChecked == true)
        {
            Player.NPCIgnore(0xC3);
        }
        else if (CheckBox_NPCIgnore.IsChecked == true && CheckBox_PoliceIgnore.IsChecked == true)
        {
            Player.NPCIgnore(0xC7);
        }
        else
        {
            Player.NPCIgnore(0x00);
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
