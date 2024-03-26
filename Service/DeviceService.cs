using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<Measurement> measurements = measurementsRaw.Select( item => new Measurement {
                StartCode = item.StartCode??"",
                MeterType = item.MeterType??"",
                MeterAddress = item.MeterAddress??"",
                ControlCode = item.ControlCode??"",
                DataLength = item.DataLength??0,
                DataId = item.DataId??"",
                Ser = item.Ser??"",
                CfUnit = item.StartCode??"",
                CummulativeFlow = item.CummulativeFlow??0,
                CfUnitSetDay = item.CfUnitSetDay??"",
                DayliCumulativeAmount = item.DayliCumulativeAmount??0,
                ReverseCfUnit = item.ReverseCfUnit??"",
                ReverseCumulativeFlow = item.ReverseCumulativeFlow??0,
                FlowRateUnit = item.FlowRateUnit??"",
                FlowRate = item.FlowRate??0,
                Temperature = item.Temperature??0,
                DevDate = item.DevDate??"",
                DevTime = item.DevTime??"",
                Status = item.Status??"",
                Valve = item.Valve??"",
                Battery = item.Battery??"",
                Battery1 = item.Battery1??"",
                Empty = item.Empty??"",
                ReverseFlow = item.ReverseFlow??"",
                OverRange = item.OverRange??"",
                WaterTemp = item.WaterTemp??"",
                Eealarm = item.Eealarm??"",
                Reserved = item.Reserved??"",
                CheckSum = item.CheckSum??"",
                EndMark = item.EndMark??"",
                RegistrationDate = item.RegistrationDate!.Value,
                GroupId = item.StartCode
            }).ToList();

            
            
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