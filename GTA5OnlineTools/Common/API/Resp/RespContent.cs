﻿namespace GTA5OnlineTools.Common.API.Resp;

public class RespContent
{
    public bool IsSuccess { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public double ExecTime { get; set; }
}