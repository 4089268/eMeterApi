using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eMeter.Models
{
    public class DeviceInfo
    {
        [JsonPropertyName("deviceAddress")]
        public string DeviceAddress { get; set; } = "";

        [JsonPropertyName("cummulativeFlow")]
        public double CummulativeFlow { get; set; }

        [JsonPropertyName("cfUnit")]
        public string CfUnit { get; set; } = "";

        [JsonPropertyName("devDate")]
        public string DevDate { get; set; } = "";

        [JsonPropertyName("devTime")]
        public string DevTime { get; set; } = "";

        [JsonPropertyName("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonPropertyName("valve")]
        public string Valve { get; set; } = "";

        [JsonPropertyName("battery")]
        public string Battery { get; set; } = "";

        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }
    }
}