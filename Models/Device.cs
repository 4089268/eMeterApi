using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMeterApi.Models
{
    public class Device
    {
        public string DeviceAddress {get; set;}
        public double? CummulativeFlow { get; set; }
        public string CfUnit {get;set;} = "l";
        public string DevDate {get;set;} = null!;
        public string DevTime {get;set;} = null!;
        public DateTime LastUpdate {get; set;}
        public string Valve {get;set;} = null!; //  "closed"
        public string Battery {get;set;} = null!; //  "normal"
        public int TotalRecords {get;set;}
        
        public Device( string address)
        {
            DeviceAddress = address;
        }

    }
}