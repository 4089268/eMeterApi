using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Entities;

namespace eMeterApi.Models.ViewModels.Devices
{
    public class DevicesIndexViewModel
    {
        public DeviceIndexFilterViewModel Filter {get;set;}
        public IEnumerable<Device> Devices {get;set;}

        public DevicesIndexViewModel(){
            Filter = new DeviceIndexFilterViewModel();
            Devices = [];
        }

        public DevicesIndexViewModel(IEnumerable<Device> devices){
            Filter = new DeviceIndexFilterViewModel();
            Devices = devices;
        }

    }
}