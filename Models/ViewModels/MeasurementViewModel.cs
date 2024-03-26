using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eMeter.Models.ViewModels
{
    public class MeasurementViewModel
    {

        public MeasurementViewModel()
        {
            Desde = DateTime.Now;
            Hasta = DateTime.Now;
        }
        public MeasurementViewModel(DateTime from , DateTime to)
        {
            Desde = from;
            Hasta = to;
        }

        [DataType(DataType.Date)]
        public DateTime Desde {get;set;}
        
        [DataType(DataType.Date)]
        public DateTime Hasta {get;set;}

        public string? DeviceAddress {get;set;}
    }
}