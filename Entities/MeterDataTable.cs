using System;
using System.Collections.Generic;

namespace eMeterApi.Entities;

public partial class MeterDataTable
{
    public string? StartCode { get; set; }

    public string? MeterType { get; set; }

    public string? MeterAddress { get; set; }

    public string? ControlCode { get; set; }

    public int? DataLength { get; set; }

    public string? DataId { get; set; }

    public string? Ser { get; set; }

    public string? CfUnit { get; set; }

    public double? CummulativeFlow { get; set; }

    public string? CfUnitSetDay { get; set; }

    public double? DayliCumulativeAmount { get; set; }

    public string? ReverseCfUnit { get; set; }

    public double? ReverseCumulativeFlow { get; set; }

    public string? FlowRateUnit { get; set; }

    public double? FlowRate { get; set; }

    public double? Temperature { get; set; }

    public string? DevDate { get; set; }

    public string? DevTime { get; set; }

    public string? Status { get; set; }

    public string? Valve { get; set; }

    public string? Battery { get; set; }

    public string? Battery1 { get; set; }

    public string? Empty { get; set; }

    public string? ReverseFlow { get; set; }

    public string? OverRange { get; set; }

    public string? WaterTemp { get; set; }

    public string? Eealarm { get; set; }

    public string? Reserved { get; set; }

    public string? CheckSum { get; set; }

    public string? EndMark { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string? GroupId { get; set; }
}
