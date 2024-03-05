using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eMeterApi.Entities
{
    public class DeviceNotification
    {

        [JsonPropertyName("confirmed")]
        public bool? Confirmed { get; set; }

        [JsonPropertyName("cr_used")]
        public string? CrUsed { get; set; }

        [JsonPropertyName("dataFrame")]
        public string? DataFrame { get; set; }

        [JsonPropertyName("data_format")]
        public string? DataFormat { get; set; }

        [JsonPropertyName("decoded")]
        public Decoded? Decoded { get; set; }

        [JsonPropertyName("decrypted")]
        public bool? Decrypted { get; set; }

        [JsonPropertyName("devaddr")]
        public int? Devaddr { get; set; }

        [JsonPropertyName("deveui")]
        public string? Deveui { get; set; }

        [JsonPropertyName("device_redundancy")]
        public int? DeviceRedundancy { get; set; }

        [JsonPropertyName("dr_used")]
        public string? DrUsed { get; set; }

        [JsonPropertyName("early")]
        public bool? Early { get; set; }

        [JsonPropertyName("fcnt")]
        public int? Fcnt { get; set; }

        [JsonPropertyName("freq")]
        public int? Freq { get; set; }

        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("live")]
        public bool? Live { get; set; }

        [JsonPropertyName("mac_msg")]
        public string? MacMsg { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("rssi")]
        public int? Rssi { get; set; }

        [JsonPropertyName("session_id")]
        public string? SessionId { get; set; }

        [JsonPropertyName("sf_used")]
        public int? SfUsed { get; set; }

        [JsonPropertyName("snr")]
        public double? Snr { get; set; }

        [JsonPropertyName("time_on_air_ms")]
        public double? TimeOnAirMs { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
    }

    public class Decoded
    {
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("msgID")]
        public int? MsgID { get; set; }

        [JsonPropertyName("msgIdDesc")]
        public string? MsgIdDesc { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }
    }
        
}

