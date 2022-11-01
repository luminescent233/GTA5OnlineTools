using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Client;

namespace GTA5OnlineTools.Modules;

/// <summary>
/// HeistPrepsWindow.xaml 的交互逻辑
/// </summary>
public partial class HeistPrepsWindow
{
    public HeistPrepsWindow()
    {
        InitializeComponent();
    }

    private void Window_HeistPreps_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_HeistPreps_Closing(object sender, CancelEventArgs e)
    {

    }

    ////////////////////////////////////////////////////

    private void AppendTextBox(string str)
    {
        AudioUtil.PlayClickSound();

        TextBox_Result.AppendText($"[{DateTime.Now:T}] {str}\n");
        TextBox_Result.ScrollToEnd();    
    }

    private void WriteStatWithDelay(string hash, int value)
    {
        Task.Run(() =>
        {
            Hacks.WriteStat(hash, value);
            Task.Delay(1000).Wait();
        });
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_BITSET1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        AppendTextBox($"重置第一款面板成功");
    }

    private void Button_H3OPT_APPROACH_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_APPROACH", 1);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫方式为 隐迹潜踪 成功");
    }

    private void Button_H3OPT_APPROACH_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_APPROACH", 2);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫方式为 兵不厌诈 成功");
    }

    private void Button_H3OPT_APPROACH_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_APPROACH", 3);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫方式为 气势汹汹 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_TARGET_0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_TARGET", 0);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫物品为 现金 成功");
    }

    private void Button_H3OPT_TARGET_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_TARGET", 1);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫物品为 黄金 成功");
    }

    private void Button_H3OPT_TARGET_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_TARGET", 2);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫物品为 艺术品 成功");
    }

    private void Button_H3OPT_TARGET_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_TARGET", 3);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"设置抢劫物品为 钻石 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_ACCESSPOINTS_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_ACCESSPOINTS", -1);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"解锁所有侦察点成功");
    }

    private void Button_H3OPT_ACCESSPOINTS_0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_ACCESSPOINTS", 0);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"取消解锁所有侦察点成功");
    }

    private void Button_H3OPT_H3OPT_POI_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_POI", -1);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"解锁所有兴趣点成功");
    }

    private void Button_H3OPT_H3OPT_POI_0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET1", 0);
        WriteStatWithDelay("_H3OPT_POI", 0);
        WriteStatWithDelay("_H3OPT_BITSET1", -1);
        AppendTextBox($"取消解锁所有兴趣点成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_BITSET0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        AppendTextBox($"重置第二款面板成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_DISRUPTSHIP_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_DISRUPTSHIP", 3);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"撤走重型武装警卫成功");
    }

    private void Button_H3OPT_KEYLEVELS_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_KEYLEVELS", 2);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"获得二级门禁卡成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_CREWWEAP_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWWEAP", 1);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置枪手等级为 卡尔·阿不拉季 5% 成功");
    }

    private void Button_H3OPT_CREWWEAP_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWWEAP", 2);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置枪手等级为 古斯塔沃·莫塔 9％ 成功");
    }

    private void Button_H3OPT_CREWWEAP_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWWEAP", 3);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置枪手等级为 查理·里德 7％ 成功");
    }

    private void Button_H3OPT_CREWWEAP_4_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWWEAP", 4);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置枪手等级为 切斯特·麦考伊 10％ 成功");
    }

    private void Button_H3OPT_CREWWEAP_5_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWWEAP", 5);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置枪手等级为 帕里克·麦克瑞利 8％ 成功");
    }

    private void Button_H3OPT_CREWWEAP_6_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWWEAP", 6);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置枪手等级为 枪手零分红 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_CREWDRIVER_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWDRIVER", 1);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置车手等级为 卡里姆·登茨 5％ 成功");
    }

    private void Button_H3OPT_CREWDRIVER_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWDRIVER", 2);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置车手等级为 塔丽娜·马丁内斯 7％ 成功");
    }

    private void Button_H3OPT_CREWDRIVER_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWDRIVER", 3);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置车手等级为 淘艾迪 9％ 成功");
    }

    private void Button_H3OPT_CREWDRIVER_4_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWDRIVER", 4);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置车手等级为 扎克·尼尔森 6％ 成功");
    }

    private void Button_H3OPT_CREWDRIVER_5_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWDRIVER", 5);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置车手等级为 切斯特·麦考伊 10％ 成功");
    }

    private void Button_H3OPT_CREWDRIVER_6_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWDRIVER", 6);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置车手等级为 车手零分红 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_CREWHACKER_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWHACKER", 1);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置黑客等级为 里奇·卢肯斯 3％ 成功");
    }

    private void Button_H3OPT_CREWHACKER_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWHACKER", 2);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置黑客等级为 克里斯汀·费尔兹 7％ 成功");
    }

    private void Button_H3OPT_CREWHACKER_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWHACKER", 3);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置黑客等级为 尤汗·布莱尔 5％ 成功");
    }

    private void Button_H3OPT_CREWHACKER_4_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWHACKER", 4);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置黑客等级为 阿维·施瓦茨曼 10％ 成功");
    }

    private void Button_H3OPT_CREWHACKER_5_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWHACKER", 5);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置黑客等级为 佩奇·哈里斯 9％ 成功");
    }

    private void Button_H3OPT_CREWHACKER_6_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_CREWHACKER", 6);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置黑客等级为 黑客零分红 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_WEAPS_0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_WEAPS", 0);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置武器类型为 类型一 成功");
    }

    private void Button_H3OPT_WEAPS_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_WEAPS", 1);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置武器类型为 类型二 成功");
    }

    private void Button_H3OPT_WEAPS_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_WEAPS", 2);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置武器类型为 类型三 成功");
    }

    private void Button_H3OPT_WEAPS_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_WEAPS", 3);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置武器类型为 类型四 成功");
    }

    private void Button_H3OPT_WEAPS_4_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_WEAPS", 4);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置武器类型为 类型五 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_H3OPT_VEH_0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_VEHS", 0);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置逃亡载具为 类型一 成功");
    }

    private void Button_H3OPT_VEH_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_VEHS", 1);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置逃亡载具为 类型二 成功");
    }

    private void Button_H3OPT_VEH_2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_VEHS", 2);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置逃亡载具为 类型三 成功");
    }

    private void Button_H3OPT_VEH_3_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_H3OPT_BITSET0", 0);
        WriteStatWithDelay("_H3OPT_VEHS", 3);
        WriteStatWithDelay("_H3OPT_BITSET0", -1);
        AppendTextBox($"设置逃亡载具为 类型四 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_FastTeleport_Click(object sender, RoutedEventArgs e)
    {
        var btnContent = (e.OriginalSource as Button).Content.ToString();

        int index = HeistPrepsConfig.FastTeleport.FindIndex(t => t.Name == btnContent);
        if (index != -1)
        {
            Teleport.SetTeleportPosition(HeistPrepsConfig.FastTeleport[index].Position);
        }

        AppendTextBox($"传送到 {btnContent} 成功");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Teleport.ToBlips(740);

        AppendTextBox($"传送到 游戏厅图标处 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_GANGOPS_FLOW_MISSION_PROG_503_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", 503);
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 229383);
        WriteStatWithDelay("_GANGOPS_FLOW_NOTIFICATIONS", 1557);
        AppendTextBox($"进入末日一分红关 成功");
    }

    private void Button_GANGOPS_FLOW_MISSION_PROG_240_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", 240);
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 229378);
        WriteStatWithDelay("_GANGOPS_FLOW_NOTIFICATIONS", 1557);
        AppendTextBox($"进入末日二分红关 成功");
    }

    private void Button_GANGOPS_FLOW_MISSION_PROG_16368_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", 16368);
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 229380);
        WriteStatWithDelay("_GANGOPS_FLOW_NOTIFICATIONS", 1557);
        AppendTextBox($"进入末日三分红关 成功");
    }

    ////////////////////////////////////////////////////

    private void Button_HEISTCOOLDOWNTIMER0_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_HEISTCOOLDOWNTIMER0", -1);
        AppendTextBox($"重置末日一冷却 成功");
    }

    private void Button_HEISTCOOLDOWNTIMER1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_HEISTCOOLDOWNTIMER1", -1);
        AppendTextBox($"重置末日二冷却 成功");
    }

    private void Button_HEISTCOOLDOWNTIMER2_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_HEISTCOOLDOWNTIMER2", -1);
        AppendTextBox($"重置末日三冷却 成功");
    }

    private void Button_GANGOPS_HEIST_STATUS_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", -1);
        AppendTextBox($"重置末日一二三任务 成功");
    }

    private void Button_GANGOPS_FLOW_NOTIFICATIONS_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 9999);

        //WriteWithStat("_GANGOPS_HEIST_STATUS", -1);
        //CoreUtils.Delay(500);

        //WriteWithStat("_GANGOPS_FLOW_NOTIFICATIONS", -1);
        //CoreUtils.Delay(500);

        AppendTextBox($"解锁重玩末日豪劫 成功");
    }

    private void Button_GANGOPS_FLOW_MISSION_PROG_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", -1);
        AppendTextBox($"跳过末日前置及准备任务 成功");
    }

    private void Button_HEIST_PLANNING_STAGE_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_HEIST_PLANNING_STAGE", -1);
        AppendTextBox($"直接进入分红关 成功");
    }
}

public class HeistPrepsConfig
{
    public static List<TeleportData.TeleportInfo> FastTeleport = new List<TeleportData.TeleportInfo>()
    {
        new TeleportData.TeleportInfo(){ Name="赌场门口", Position=new Vector3 { X=911.072f, Y=53.321f, Z=80.893f } },
        new TeleportData.TeleportInfo(){ Name="监控和安保人员", Position=new Vector3 { X=1089.614f, Y=215.696f, Z=-49.200f } },
        new TeleportData.TeleportInfo(){ Name="门禁系统", Position=new Vector3 { X=1117.732f, Y=214.123f, Z=-49.440f } },
        new TeleportData.TeleportInfo(){ Name="赌场后门", Position=new Vector3 { X=993.162f, Y=86.234f, Z=80.990f } },
    };
}
