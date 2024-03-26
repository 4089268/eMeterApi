using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Entities;
using eMeter.Models;

namespace eMeter.Helpers
{
    public class MeasurementAdapter
    {

        public static Measurement FromMeterDataTable(MeterDataTable item)
        {
            return new Measurement
            {
                StartCode = item.StartCode ?? "",
                MeterType = item.MeterType ?? "",
                MeterAddress = item.MeterAddress ?? "",
                ControlCode = item.ControlCode ?? "",
                DataLength = item.DataLength ?? 0,
                DataId = item.DataId ?? "",
                Ser = item.Ser ?? "",
                CfUnit = item.StartCode ?? "",
                CummulativeFlow = item.CummulativeFlow ?? 0,
                CfUnitSetDay = item.CfUnitSetDay ?? "",
                DayliCumulativeAmount = item.DayliCumulativeAmount ?? 0,
                ReverseCfUnit = item.ReverseCfUnit ?? "",
                ReverseCumulativeFlow = item.ReverseCumulativeFlow ?? 0,
                FlowRateUnit = item.FlowRateUnit ?? "",
                FlowRate = item.FlowRate ?? 0,
                Temperature = item.Temperature ?? 0,
                DevDate = item.DevDate ?? "",
                DevTime = item.DevTime ?? "",
                Status = item.Status ?? "",
                Valve = item.Valve ?? "",
                Battery = item.Battery ?? "",
                Battery1 = item.Battery1 ?? "",
                Empty = item.Empty ?? "",
                ReverseFlow = item.ReverseFlow ?? "",
                OverRange = item.OverRange ?? "",
                WaterTemp = item.WaterTemp ?? "",
                Eealarm = item.Eealarm ?? "",
                Reserved = item.Reserved ?? "",
                CheckSum = item.CheckSum ?? "",
                EndMark = item.EndMark ?? "",
                RegistrationDate = item.RegistrationDate!.Value,
                GroupId = item.StartCode
            };
        }
    
        
    }
}