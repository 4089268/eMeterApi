using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eMeter.Helpers;
using eMeter.Models;
using eMeterApi.Data;
using eMeterApi.Entities;
using eMeterApi.Models;
using System.Collections.Immutable;

namespace eMeter.Service
{
    public class DeviceService
    {

        private readonly EMeterContext eMeterContext;
        private readonly ILogger<DeviceService> logger;

        public DeviceService( EMeterContext eMeterContext, ILogger<DeviceService> logger){
            this.eMeterContext = eMeterContext;
            this.logger = logger;
        }

        public IEnumerable<Device> GetDevices(out int totalItems, int chunk = 25, int page = 0, IEnumerable<string>? groupsId = null)
        {

            var devicesQuery = eMeterContext.Devices.AsQueryable();

            if(groupsId != null)
            {
                devicesQuery = devicesQuery.Where( device => 
                    groupsId.Select(groupId => 
                        groupId.ToLower()
                    ).Contains(device.GroupId.ToLower())
                );
            }

            totalItems = devicesQuery.Count();
            
            if( chunk == 0){
                return devicesQuery.ToList();
            }

            return devicesQuery.Skip(chunk * page )
                .Take(chunk)
                .ToList();
        }

        public DeviceDetails? GetDeviceInfo(string deviceAddress)
        {
            // Get device info
            var device = eMeterContext.Devices.Where( item => item.MeterAddress == deviceAddress).FirstOrDefault();
            if(device == null){
                logger.LogWarning($"Device addres {deviceAddress} was not found in the database at DeviceService.GetDeviceInfo");
                return null;
            }

            // Get las measurements
            var measurementsRaw = eMeterContext.MeterDataTables
                .OrderByDescending( item => item.RegistrationDate)
                .Where( item => item.MeterAddress == deviceAddress )
                .Take(25)
                .ToList();
            // Process data
            List<Measurement> measurements = measurementsRaw.Select( item => MeasurementAdapter.FromMeterDataTable(item) ).ToList();

            // Prepare response
            return new DeviceDetails {
                DeviceAddress = deviceAddress,
                Device = device,
                Measurement = measurements
            };
        }

    }
}