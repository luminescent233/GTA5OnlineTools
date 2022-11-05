using CommunityToolkit.Mvvm.ComponentModel;

namespace GTA5OnlineTools.Models;

public class KiddionModel : ObservableObject
{
    private int _index;
    /// <summary>
    /// 索引
    /// </summary>
    public int Index
    {
        get => _index;
        set => SetProperty(ref _index, value);
    }

    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _value;
    /// <summary>
    /// 值
    /// </summary>
    public string Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }
}
