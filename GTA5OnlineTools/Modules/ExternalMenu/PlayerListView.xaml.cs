using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Data;

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
                    RID = item.RockstarId
                });
            }
            else
            {
                ListBox_PlayerList.Items.Add(new NetPlayer()
                {
                    Index = ++index,
                    Avatar = url,
                    Name = item.PlayerName,
                    RID = item.RockstarId
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

    private void ListBox_PlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TextBox_PlayerInfo.Clear();

        var index = ListBox_PlayerList.SelectedIndex;
        if (index != -1)
        {
            TextBox_PlayerInfo.AppendText($"战局房主 : {NetPlayerDatas[index].IsHost}\n\n");

            TextBox_PlayerInfo.AppendText($"玩家RID : {NetPlayerDatas[index].RockstarId}\n");
            TextBox_PlayerInfo.AppendText($"玩家昵称 : {NetPlayerDatas[index].PlayerName}\n\n");

            TextBox_PlayerInfo.AppendText($"当前生命值 : {NetPlayerDatas[index].Health:0.0}\n");
            TextBox_PlayerInfo.AppendText($"最大生命值 : {NetPlayerDatas[index].MaxHealth:0.0}\n\n");

            TextBox_PlayerInfo.AppendText($"无敌状态 : {NetPlayerDatas[index].GodMode}\n");
            TextBox_PlayerInfo.AppendText($"无布娃娃 : {NetPlayerDatas[index].NoRagdoll}\n\n");

            TextBox_PlayerInfo.AppendText($"通缉等级 : {NetPlayerDatas[index].WantedLevel}\n");
            TextBox_PlayerInfo.AppendText($"奔跑速度 : {NetPlayerDatas[index].RunSpeed:0.0}\n\n");

            TextBox_PlayerInfo.AppendText($"X : {NetPlayerDatas[index].Position.X:0.000}\n");
            TextBox_PlayerInfo.AppendText($"Y : {NetPlayerDatas[index].Position.Y:0.000}\n");
            TextBox_PlayerInfo.AppendText($"Z : {NetPlayerDatas[index].Position.Z:0.000}\n");
        }
    }
}
