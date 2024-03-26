#pragma warning disable CS8618 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eMeterApi.Models;

namespace eMeter.Models
{
    public class DeviceDetails
    {
        [JsonPropertyName("deviceAddress")]
        public string DeviceAddress { get; set; }

        [JsonPropertyName("device")]
        public Device Device { get; set; }

        [JsonPropertyName("measurement")]
        public List<Measurement> Measurement { get; set; }
    }
}