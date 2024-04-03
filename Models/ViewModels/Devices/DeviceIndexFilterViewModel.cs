using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace eMeterApi.Models.ViewModels.Devices
{
    public class DeviceIndexFilterViewModel
    {
        [DisplayName("Esatus de Valvula")]
        public string VS {get;set;} = "";

        [DisplayName("Esatus de Bateria")]
        public string BS {get;set;} = "";

        [DisplayName("Proyecto")]
        public string PK {get;set;} = "";

        public int Page {get;set;} = 0;
        public int Chunk {get;set;} = 25;
        public string? S {get;set;}
    }
}