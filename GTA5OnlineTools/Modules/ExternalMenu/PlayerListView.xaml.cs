using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Data;
using System;

namespace GTA5OnlineTools.Modules.ExternalMenu;

/// <summary>
/// PlayerListView.xaml 的交互逻辑
/// </summary>
public partial class PlayerListView : UserControl
{
    // 显示玩家列表
    private List<NetPlayerData> NetPlayerDatas = new();

    public PlayerListView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {

    }

    private void Button_RefreshPlayerList_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        NetPlayerDatas.Clear();
        ListBox_PlayerList.Items.Clear();

        long pCNetworkPlayerMgr = Memory.Read<long>(Pointers.NetworkPlayerMgrPTR);

        long pCNetGamePlayerLocal = Memory.Read<long>(pCNetworkPlayerMgr + Offsets.CNetworkPlayerMgr_CNetGamePlayerLocal);
        long pCPlayerInfoLocal = Memory.Read<long>(pCNetGamePlayerLocal + Offsets.CNetworkPlayerMgr_CNetGamePlayer_CPlayerInfo);
        long oLocalHostToken = Memory.Read<long>(pCPlayerInfoLocal + Offsets.CNetworkPlayerMgr_CNetGamePlayer_HostToken);

        for (int i = 0; i < 32; i++)
        {
            long pCNetGamePlayer = Memory.Read<long>(pCNetworkPlayerMgr + Offsets.CNetworkPlayerMgr_CNetGamePlayer + i * 0x08);
            if (!Memory.IsValid(pCNetGamePlayer))
                continue;

            long pCPlayerInfo = Memory.Read<long>(pCNetGamePlayer + Offsets.CNetworkPlayerMgr_CNetGamePlayer_CPlayerInfo);
            if (!Memory.IsValid(pCPlayerInfo))
                continue;

            long oHostToken = Memory.Read<long>(pCPlayerInfo + Offsets.CNetworkPlayerMgr_CNetGamePlayer_HostToken);

            long pCPed = Memory.Read<long>(pCPlayerInfo + Offsets.CNetworkPlayerMgr_CNetGamePlayer_CPed);
            if (!Memory.IsValid(pCPed))
                continue;

            ////////////////////////////////////////////

            NetPlayerDatas.Add(new NetPlayerData()
            {
                RockstarId = Memory.Read<long>(pCPlayerInfo + Offsets.CNetworkPlayerMgr_CNetGamePlayer_RockstarId),
                PlayerName = Memory.ReadString(pCPlayerInfo + Offsets.CNetworkPlayerMgr_CNetGamePlayer_Name, 20),

                IsHost = oHostToken == oLocalHostToken,
                HostToken = oHostToken,

                Health = Memory.Read<float>(pCPed + Offsets.CPed_Health),
                MaxHealth = Memory.Read<float>(pCPed + Offsets.CPed_HealthMax),
                GodMode = Memory.Read<byte>(pCPed + Offsets.CPed_God) == 0x01,
                NoRagdoll = Memory.Read<byte>(pCPed + Offsets.CPed_Ragdoll) == 0x01,
                Position = Memory.Read<Vector3>(pCPed + Offsets.CPed_VisualX),

                WantedLevel = Memory.Read<byte>(pCPlayerInfo + 0x888),
                RunSpeed = Memory.Read<float>(pCPlayerInfo + 0xCF0)
            });
        }

        var index = 0;
        foreach (var item in NetPlayerDatas)
        {
            var url = "https://prod.cloud.rockstargames.com/members/sc/5605/" + item.RockstarId + "/publish/gta5/mpchars/0.png";

            if (item.IsHost)
            {
                ListBox_PlayerList.Items.Add(new NetPlayer()
                {
                    Index = ++index,
                    Avatar = url,
                    Name = $"{item.PlayerName} [房主]",
                    RID = item.RockstarId,
                    GodMode = item.GodMode ? "无敌" : ""
                });
            }
            else
            {
                ListBox_PlayerList.Items.Add(new NetPlayer()
                {
                    Index = ++index,
                    Avatar = url,
                    Name = item.PlayerName,
                    RID = item.RockstarId,
                    GodMode = item.GodMode ? "无敌" : ""
                });
            }
        }
    }

    private void Button_TeleportSelectedPlayer_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        if (ListBox_PlayerList.SelectedItem != null)
        {
            var index = ListBox_PlayerList.SelectedIndex;
            if (index != -1)
            {
                Teleport.SetTeleportPosition(NetPlayerDatas[index].Position);
            }
        }
    }

    private void PlayerInfoAppend(string msg)
    {
        TextBox_PlayerInfo.AppendText($"{msg}\n");
    }

    private void ListBox_PlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TextBox_PlayerInfo.Clear();

        var index = ListBox_PlayerList.SelectedIndex;
        if (index != -1)
        {
            PlayerInfoAppend($"战局房主 : {NetPlayerDatas[index].IsHost}\n");

            PlayerInfoAppend($"玩家RID : {NetPlayerDatas[index].RockstarId}");
            PlayerInfoAppend($"玩家昵称 : {NetPlayerDatas[index].PlayerName}\n");

            PlayerInfoAppend($"当前生命值 : {NetPlayerDatas[index].Health:0.0}");
            PlayerInfoAppend($"最大生命值 : {NetPlayerDatas[index].MaxHealth:0.0}\n");

            PlayerInfoAppend($"无敌状态 : {NetPlayerDatas[index].GodMode}");
            PlayerInfoAppend($"无布娃娃 : {NetPlayerDatas[index].NoRagdoll}\n");

            PlayerInfoAppend($"通缉等级 : {NetPlayerDatas[index].WantedLevel}");
            PlayerInfoAppend($"奔跑速度 : {NetPlayerDatas[index].RunSpeed:0.0}\n");

            PlayerInfoAppend($"X坐标 : {NetPlayerDatas[index].Position.X:0.000}");
            PlayerInfoAppend($"Y坐标 : {NetPlayerDatas[index].Position.Y:0.000}");
            PlayerInfoAppend($"Z坐标 : {NetPlayerDatas[index].Position.Z:0.000}");
        }
    }
}
