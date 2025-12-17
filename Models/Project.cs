using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts.Models;

namespace eMeter.Models
{
    public class Project : IProject
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("proyecto")]
        public string Proyecto { get; set; } = "";

        [JsonPropertyName("clave")]
        public string Clave { get; set; } = "";

        [JsonPropertyName("oficinaId")]
        public int OficinaId { get; set; } = 0;
    }
}