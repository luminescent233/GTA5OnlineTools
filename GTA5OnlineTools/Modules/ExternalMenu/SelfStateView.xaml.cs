using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Client;
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

    /////////////////////////////////////////////////////////

    /// <summary>
    /// 判断程序是否在运行，用于结束线程
    /// </summary>
    private bool IsAppRunning = true;

    private bool Toggle_NoCollision = false;

    private float Forward_Distance = 1.5f;
    private bool Player_GodMode = false;
    private bool Player_AntiAFK = false;
    private bool Player_NoRagdoll = false;
    private bool Player_NoCollision = false;

    private bool Vehicle_GodMode = false;
    private bool Vehicle_Seatbelt = false;

    private bool Auto_ClearWanted = false;
    private bool Auto_KillNPC = false;
    private bool Auto_KillHostilityNPC = false;
    private bool Auto_KillPolice = false;

    /////////////////////////////////////////////////////////

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
        IsAppRunning = false;

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
                    Teleport.MovingFoward(Forward_Distance);
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
                    Toggle_NoCollision = !Toggle_NoCollision;

                    Player.NoCollision(Toggle_NoCollision);
                    Player_NoCollision = Toggle_NoCollision;

                    if (Toggle_NoCollision)
                        Console.Beep(600, 75);
                    else
                        Console.Beep(500, 75);
                }
                break;
        }
    }

    private void SelfStateMainThread()
    {
        while (IsAppRunning)
        {
            long pCPedFactory = Memory.Read<long>(Globals.WorldPTR);
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
            if (Player_GodMode)
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
            if (Player_AntiAFK)
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
            if (Player_NoRagdoll)
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
            if (Player_NoCollision)
            {
                long pointer = Memory.Read<long>(pCNavigation + 0x10);
                pointer = Memory.Read<long>(pointer + 0x20);
                pointer = Memory.Read<long>(pointer + 0x70);
                pointer = Memory.Read<long>(pointer + 0x00);
                Memory.Write(pointer + 0x2C, -1.0f);
            }

            // 安全带
            if (Vehicle_Seatbelt)
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
                if (Vehicle_GodMode)
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
        while (IsAppRunning)
        {
            // 自动消星
            if (Auto_ClearWanted)
                Player.WantedLevel(0x00);

            long pCReplayInterface = Memory.Read<long>(Globals.ReplayInterfacePTR);
            long pCPedInterface = Memory.Read<long>(pCReplayInterface + Offsets.CReplayInterface_CPedInterface);
            int oMaxPeds = Memory.Read<int>(pCPedInterface + Offsets.CReplayInterface_CPedInterface_MaxPeds);

            for (int i = 0; i < oMaxPeds; i++)
            {
                long pCPedList = Memory.Read<long>(pCPedInterface + Offsets.CReplayInterface_CPedInterface_CPedList);

                long pCPed = Memory.Read<long>(pCPedList + i * 0x10);
                if (!Memory.IsValid(pCPed))
                    continue;

                // 跳过玩家
                long pCPlayerInfo = Memory.Read<long>(pCPed + Offsets.CPed_CPlayerInfo);
                if (Memory.IsValid(pCPlayerInfo))
                    continue;

                // 自动击杀NPC
                if (Auto_KillNPC)
                    Memory.Write(pCPed + Offsets.CPed_Health, 0.0f);

                // 自动击杀敌对NPC
                if (Auto_KillHostilityNPC)
                {
                    byte oHostility = Memory.Read<byte>(pCPed + Offsets.CPed_Hostility);
                    if (oHostility > 0x01)
                    {
                        Memory.Write(pCPed + Offsets.CPed_Health, 0.0f);
                    }
                }

                // 自动击杀警察
                if (Auto_KillPolice)
                {
                    int ped_type = Memory.Read<int>(pCPed + Offsets.CPed_Ragdoll);
                    ped_type = ped_type << 11 >> 25;

                    if (ped_type == (int)EnumData.PedTypes.COP ||
                        ped_type == (int)EnumData.PedTypes.SWAT ||
                        ped_type == (int)EnumData.PedTypes.ARMY)
                    {
                        Memory.Write(pCPed + Offsets.CPed_Health, 0.0f);
                    }
                }
            }

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
        Player_GodMode = CheckBox_PlayerGodMode.IsChecked == true;
    }

    private void CheckBox_AntiAFK_Click(object sender, RoutedEventArgs e)
    {
        Player.AntiAFK(CheckBox_AntiAFK.IsChecked == true);
        Player_AntiAFK = CheckBox_AntiAFK.IsChecked == true;
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
        Player_NoRagdoll = CheckBox_NoRagdoll.IsChecked == true;
    }

    private void CheckBox_NPCIgnore_Click(object sender, RoutedEventArgs e)
    {
        if (CheckBox_NPCIgnore.IsChecked == true && CheckBox_PoliceIgnore.IsChecked == false)
        {
            Player.NPCIgnore(0x040000);
        }
        else if (CheckBox_NPCIgnore.IsChecked == false && CheckBox_PoliceIgnore.IsChecked == true)
        {
            Player.NPCIgnore(0xC30000);
        }
        else if (CheckBox_NPCIgnore.IsChecked == true && CheckBox_PoliceIgnore.IsChecked == true)
        {
            Player.NPCIgnore(0xC70000);
        }
        else
        {
            Player.NPCIgnore(0x00);
        }
    }

    private void CheckBox_AutoClearWanted_Click(object sender, RoutedEventArgs e)
    {
        Player.WantedLevel(0x00);
        Auto_ClearWanted = CheckBox_AutoClearWanted.IsChecked == true;
    }

    private void CheckBox_AutoKillNPC_Click(object sender, RoutedEventArgs e)
    {
        World.KillAllNPC(false);
        Auto_KillNPC = CheckBox_AutoKillNPC.IsChecked == true;
    }

    private void CheckBox_AutoKillHostilityNPC_Click(object sender, RoutedEventArgs e)
    {
        World.KillAllNPC(true);
        Auto_KillHostilityNPC = CheckBox_AutoKillHostilityNPC.IsChecked == true;
    }

    private void CheckBox_AutoKillPolice_Click(object sender, RoutedEventArgs e)
    {
        World.KillAllPolice();
        Auto_KillPolice = CheckBox_AutoKillPolice.IsChecked == true;
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
        Forward_Distance = (float)Slider_MovingFoward.Value;
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
        Toggle_NoCollision = SelfStateModel.IsHotKeyNoCollision;

        Player.NoCollision(Toggle_NoCollision);
        Player_NoCollision = Toggle_NoCollision;
    }
}
