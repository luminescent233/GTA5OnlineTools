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
    private List<PlayerData> playerData = new();

    public PlayerListView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {

    }

    private void ListBox_PlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TextBox_PlayerInfo.Clear();

        var index = ListBox_PlayerList.SelectedIndex;
        if (index != -1)
        {
            TextBox_PlayerInfo.AppendText($"战局房主 : {playerData[index].PlayerInfo.Host}\n\n");

            TextBox_PlayerInfo.AppendText($"玩家RID : {playerData[index].RID}\n");
            TextBox_PlayerInfo.AppendText($"玩家昵称 : {playerData[index].Name}\n\n");

            TextBox_PlayerInfo.AppendText($"当前生命值 : {playerData[index].PlayerInfo.Health:0.0}\n");
            TextBox_PlayerInfo.AppendText($"最大生命值 : {playerData[index].PlayerInfo.MaxHealth:0.0}\n\n");

            TextBox_PlayerInfo.AppendText($"无敌状态 : {playerData[index].PlayerInfo.GodMode}\n");
            TextBox_PlayerInfo.AppendText($"无布娃娃 : {playerData[index].PlayerInfo.NoRagdoll}\n\n");

            TextBox_PlayerInfo.AppendText($"通缉等级 : {playerData[index].PlayerInfo.WantedLevel}\n");
            TextBox_PlayerInfo.AppendText($"奔跑速度 : {playerData[index].PlayerInfo.RunSpeed:0.0}\n\n");

            TextBox_PlayerInfo.AppendText($"X : {playerData[index].PlayerInfo.V3Pos.X:0.0000}\n");
            TextBox_PlayerInfo.AppendText($"Y : {playerData[index].PlayerInfo.V3Pos.Y:0.0000}\n");
            TextBox_PlayerInfo.AppendText($"Z : {playerData[index].PlayerInfo.V3Pos.Z:0.0000}\n");
        }
    }

    private void Button_RefreshPlayerList_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        playerData.Clear();
        ListBox_PlayerList.Items.Clear();

        for (int i = 0; i < 32; i++)
        {
            long pCNetGamePlayer = GTA5Mem.Read<long>(General.NetworkPlayerMgrPTR, new int[] { 0x180 + (i * 8) });
            if (!GTA5Mem.IsValid(pCNetGamePlayer))
                continue;

            long pCPlayerInfo = GTA5Mem.Read<long>(pCNetGamePlayer + 0xA0);
            if (!GTA5Mem.IsValid(pCPlayerInfo))
                continue;

            long pCPed = GTA5Mem.Read<long>(pCPlayerInfo + 0x01E8);
            if (!GTA5Mem.IsValid(pCPed))
                continue;

            long pCNavigation = GTA5Mem.Read<long>(pCPed + 0x30, null);
            if (!GTA5Mem.IsValid(pCNavigation))
                continue;

            ////////////////////////////////////////////

            playerData.Add(new PlayerData()
            {
                RID = GTA5Mem.Read<long>(pCPlayerInfo + 0x90),
                Name = GTA5Mem.ReadString(pCPlayerInfo + 0xA4, null, 20),

                PlayerInfo = new PlayerInfo()
                {
                    Host = Hacks.ReadGA<int>(1892703 + (i * 599) + 10) == 1 ? true : false,
                    Health = GTA5Mem.Read<float>(pCPed + 0x280),
                    MaxHealth = GTA5Mem.Read<float>(pCPed + 0x2A0),
                    GodMode = GTA5Mem.Read<byte>(pCPed + 0x189) == 0x01 ? true : false,
                    NoRagdoll = GTA5Mem.Read<byte>(pCPed + 0x10B8) == 0x01 ? true : false,
                    WantedLevel = GTA5Mem.Read<byte>(pCPlayerInfo + 0x888),
                    RunSpeed = GTA5Mem.Read<float>(pCPlayerInfo + 0xCF0),
                    V3Pos = GTA5Mem.Read<Vector3>(pCNavigation + 0x50)
                },
            });
        }

        int index = 0;
        foreach (var item in playerData)
        {
            if (item.PlayerInfo.Host)
            {
                index++;
                ListBox_PlayerList.Items.Add($"{index}  {item.Name} [房主]");
            }
            else
            {
                index++;
                ListBox_PlayerList.Items.Add($"{index}  {item.Name}");
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
                Teleport.SetTeleportPosition(playerData[index].PlayerInfo.V3Pos);
            }
        }
    }
}
