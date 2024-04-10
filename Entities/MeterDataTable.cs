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

    public string DeviceId { get; set; } = null!;

    public static MeterDataTable FromMeterData( MeterData meterData){
        var item = new MeterDataTable
        {
            StartCode = meterData.StartCode,
            MeterType = meterData.MeterType,
            MeterAddress = meterData.MeterAddress,
            ControlCode = meterData.ControlCode,
            DataLength = meterData.DataLenght,
            DataId = meterData.DataId,
            Ser = meterData.Ser,
            CfUnit = meterData.CfUnit,
            CummulativeFlow = meterData.CummulativeFlow,
            CfUnitSetDay = meterData.CfUnitSetDay,
            DayliCumulativeAmount = meterData.DayliCumulativeAmount,
            ReverseCfUnit = meterData.ReverseCfUnit,
            ReverseCumulativeFlow = meterData.ReverseCumulativeFlow,
            FlowRateUnit = meterData.FlowRateUnit,
            FlowRate = meterData.FlowRate,
            Temperature = meterData.Temperature,
            DevDate = meterData.DevDate,
            DevTime = meterData.DevTime,
            Status = meterData.Status,
            Valve = meterData.Valve,
            Battery = meterData.Battery,
            Battery1 = meterData.Battery1,
            Empty = meterData.Empty,
            ReverseFlow = meterData.ReverseFlow,
            OverRange = meterData.OverRange,
            WaterTemp = meterData.WaterTemp,
            Eealarm = meterData.EEAlarm,
            Reserved = meterData.ReverseFlow,
            CheckSum = meterData.CheckSume,
            EndMark = meterData.EndMark,
            RegistrationDate = DateTime.Now
        };
        return item;
    }

}
