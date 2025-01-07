using System;
using System.Collections.Generic;

namespace eMeterApi.Entities;

public partial class Device
{
    public string MeterAddress { get; set; } = null!;

    public string? CfUnit { get; set; }

    public double? CummulativeFlow { get; set; }

    public string? FlowRateUnit { get; set; }

    public double? FlowRate { get; set; }

    public double? Temperature { get; set; }

    public string? DevDate { get; set; }

    public string? DevTime { get; set; }

    public string Valve { get; set; } = null!;

    public string Battery { get; set; } = null!;

    public string ReverseFlow { get; set; } = null!;

    public string WaterTemp { get; set; } = null!;

    public string GroupId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string DeviceId { get; set; } = null!;

    public override string ToString()
    {
        return $"DeviceId: {DeviceId}, MeterAddress: {MeterAddress}, CfUnit: {CfUnit}, CummulativeFlow: {CummulativeFlow}, FlowRateUnit: {FlowRateUnit}, FlowRate: {FlowRate}, Temperature: {Temperature}, DevDate: {DevDate}, DevTime: {DevTime}, Valve: {Valve}, Battery: {Battery}, ReverseFlow: {ReverseFlow}, WaterTemp: {WaterTemp}, GroupId: {GroupId}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
}
