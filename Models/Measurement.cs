#pragma warning disable CS8618 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eMeter.Models
{
    public class Measurement
    {
        [JsonPropertyName("startCode")]
        public string StartCode { get; set; }

        [JsonPropertyName("meterType")]
        public string MeterType { get; set; }

        [JsonPropertyName("meterAddress")]
        public string MeterAddress { get; set; }

        [JsonPropertyName("controlCode")]
        public string ControlCode { get; set; }

        [JsonPropertyName("dataLength")]
        public int DataLength { get; set; }

        [JsonPropertyName("dataId")]
        public string DataId { get; set; }

        [JsonPropertyName("ser")]
        public string Ser { get; set; }

        [JsonPropertyName("cfUnit")]
        public string CfUnit { get; set; }

        [JsonPropertyName("cummulativeFlow")]
        public double CummulativeFlow { get; set; }

        [JsonPropertyName("cfUnitSetDay")]
        public string CfUnitSetDay { get; set; }

        [JsonPropertyName("dayliCumulativeAmount")]
        public double DayliCumulativeAmount { get; set; }

        [JsonPropertyName("reverseCfUnit")]
        public string ReverseCfUnit { get; set; }

        [JsonPropertyName("reverseCumulativeFlow")]
        public double ReverseCumulativeFlow { get; set; }

        [JsonPropertyName("flowRateUnit")]
        public string FlowRateUnit { get; set; }

        [JsonPropertyName("flowRate")]
        public double FlowRate { get; set; }

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("devDate")]
        public string DevDate { get; set; }

        [JsonPropertyName("devTime")]
        public string DevTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("valve")]
        public string Valve { get; set; }

        [JsonPropertyName("battery")]
        public string Battery { get; set; }

        [JsonPropertyName("battery1")]
        public string Battery1 { get; set; }

        [JsonPropertyName("empty")]
        public string Empty { get; set; }

        [JsonPropertyName("reverseFlow")]
        public string ReverseFlow { get; set; }

        [JsonPropertyName("overRange")]
        public string OverRange { get; set; }

        [JsonPropertyName("waterTemp")]
        public string WaterTemp { get; set; }

        [JsonPropertyName("eealarm")]
        public string Eealarm { get; set; }

        [JsonPropertyName("reserved")]
        public string Reserved { get; set; }

        [JsonPropertyName("checkSum")]
        public string CheckSum { get; set; }

        [JsonPropertyName("endMark")]
        public string EndMark { get; set; }

        [JsonPropertyName("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonPropertyName("groupId")]
        public string? GroupId { get; set; }
    }


}