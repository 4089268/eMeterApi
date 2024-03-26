using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeter.Helpers;
using eMeter.Models;
using eMeterApi.Data;
using eMeterApi.Models;

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

        public IEnumerable<Device> GetDevices(out int totalItems, int chunk = 25, int page = 0)
        {

            var devicesDataQuery = eMeterContext.MeterDataTables
                .Where( item => item.MeterAddress != null)
                .OrderByDescending( item => item.RegistrationDate)
                .GroupBy( item => item.MeterAddress);

            totalItems = devicesDataQuery.Count();

            var devicesData = devicesDataQuery
                .Select( g =>  new { DeviceAddress = g.Key, Info = g.FirstOrDefault(), TotalRecords = g.Count() })
                .Skip( chunk * page)
                .Take( chunk )
                .ToList();

            var devicesResponse = new List<Device>();

            foreach( var deviceGroup in devicesData)
            {
                if(deviceGroup.Info != null){
                    var deviceInfo = new Device( deviceGroup.DeviceAddress! )
                    {
                        CummulativeFlow = deviceGroup.Info.CummulativeFlow,
                        DevDate = deviceGroup.Info.DevDate!,
                        DevTime = deviceGroup.Info.DevTime!,
                        Valve = deviceGroup.Info.Valve!.ToUpper(),
                        Battery = deviceGroup.Info.Battery!.ToUpper(),
                        LastUpdate = deviceGroup.Info.RegistrationDate!.Value,
                        TotalRecords = deviceGroup.TotalRecords
                    };
                    // TODO: Convert meterDataTable.CfUnit
                    // deviceInfo.CfUnit = meterDataTable.CfUnit;
                    devicesResponse.Add( deviceInfo);
                }
            }

            return devicesResponse;

        }

        public DeviceDetails? GetDeviceInfo(string deviceAddress)
        {

             var deviceInfoRaw = eMeterContext.MeterDataTables
                .OrderByDescending( item => item.RegistrationDate)
                .Where( item => item.MeterAddress == deviceAddress )
                .FirstOrDefault();

            if( deviceInfoRaw == null){
                logger.LogWarning($"Device addres {deviceAddress} was not found in the database at DeviceService.GetDeviceInfo");
                return null;
            }

            var measurementsQuery = eMeterContext.MeterDataTables
                .OrderByDescending( item => item.RegistrationDate)
                .Where( item => item.MeterAddress == deviceAddress );
            
            var totalItems = measurementsQuery.Count();

            // TODO: Create adapter
            var measurementsRaw = measurementsQuery.Take(25).ToList();
            List<Measurement> measurements = measurementsRaw.Select( item => MeasurementAdapter.FromMeterDataTable(item) ).ToList();

            
            
            // Prepare response
            return new DeviceDetails {
                DeviceAddress = deviceAddress,
                Device = new Device( deviceInfoRaw!.MeterAddress! )
                {
                    CummulativeFlow = deviceInfoRaw.CummulativeFlow,
                    DevDate = deviceInfoRaw.DevDate!,
                    DevTime = deviceInfoRaw.DevTime!,
                    Valve = deviceInfoRaw.Valve!.ToUpper(),
                    Battery = deviceInfoRaw.Battery!.ToUpper(),
                    LastUpdate = deviceInfoRaw.RegistrationDate!.Value,
                    TotalRecords = totalItems
                },
                Measurement = measurements
            };

            
        }


    }
}