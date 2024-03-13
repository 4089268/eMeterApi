using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using eMeterApi.Data;
using eMeterApi.Entities;
using eMeterApi.Models;
using eMeterApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace eMeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {

        private readonly ILogger<MeasurementController> logger;
        private readonly EMeterContext eMeterContext;

        public MeasurementController( ILogger<MeasurementController> logger, EMeterContext eMeterContext){
            this.logger = logger;
            this.eMeterContext = eMeterContext;
        }
        

        /// <summary>
        /// Retrive all stored measures
        /// </summary>
        /// <returns> List of  stored measures </returns>
        /// <response code="200">Returns the data</response>
        [HttpGet]
        public IActionResult GetMeasurement( [FromQuery] int chunk = 25, [FromQuery] int page = 0, [FromQuery] string? deviceAddress = "", [FromQuery] string? from = "", [FromQuery] string? to = "" )
        {
            
            var query = eMeterContext.MeterDataTables.OrderByDescending( e => e.RegistrationDate).AsQueryable();
            if( !string.IsNullOrEmpty(deviceAddress) ){
                query = query.Where( item => item.MeterAddress == deviceAddress);
            }

            if( !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) ){
                if(DateTime.TryParse(from, out var _from) && DateTime.TryParse(to, out var _to)){
                    query = query.Where(item => _from.Date <= item.RegistrationDate!.Value.Date && item.RegistrationDate!.Value.Date <= _to.Date);

                }
                else {
                    return BadRequest( new {
                        title = "Dates are not valid",
                        message = "Invalid date format or values provided for 'from' and 'to'."
                    });
                }
            }


            var totalItems = query.Count();

            IEnumerable<MeterDataTable> data = Array.Empty<MeterDataTable>();
            if( chunk == 0){
                data = query.ToList();
            }else{
                data = query
                    .Skip( chunk * page)
                    .Take( chunk)
                    .ToList();
            }

            return Ok( new EnumerableResponse<MeterDataTable>(){
                Data = data,
                ChunkSize = chunk,
                Page=page,
                TotalItems = totalItems
            }) ;
        }

        [HttpGet]
        [Route("Devices")]
        public IActionResult GetDevices( [FromQuery] int chunk = 25, [FromQuery] int page = 0 )
        {
            
            var devicesDataQuery = eMeterContext.MeterDataTables
                .Where( item => item.MeterAddress != null)
                .OrderByDescending( item => item.RegistrationDate)
                .GroupBy( item => item.MeterAddress);

            var totalItems = devicesDataQuery.Count();

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

            return Ok( new EnumerableResponse<Device>(){
                Data = devicesResponse,
                ChunkSize = chunk,
                Page = page,
                TotalItems = totalItems
            });
            
        }

        [HttpGet]
        [Route("Devices/{deviceAddress}")]
        public IActionResult GetDeviceInfo( [FromRoute] string deviceAddress )
        {
            
            var deviceInfoRaw = eMeterContext.MeterDataTables
                .OrderByDescending( item => item.RegistrationDate)
                .Where( item => item.MeterAddress == deviceAddress )
                .FirstOrDefault();

            if( deviceInfoRaw == null){
                return BadRequest( new{
                    message = $"Device address {deviceAddress} not found"
                });
            }

            var measurementsQuery = eMeterContext.MeterDataTables
                .OrderByDescending( item => item.RegistrationDate)
                .Where( item => item.MeterAddress == deviceAddress );
            
            var totalItems = measurementsQuery.Count();

            var measurements = measurementsQuery
                .Take(25)
                .ToImmutableList();
            
            var deviceInfo = new Device( deviceInfoRaw!.MeterAddress! )
            {
                CummulativeFlow = deviceInfoRaw.CummulativeFlow,
                DevDate = deviceInfoRaw.DevDate!,
                DevTime = deviceInfoRaw.DevTime!,
                Valve = deviceInfoRaw.Valve!.ToUpper(),
                Battery = deviceInfoRaw.Battery!.ToUpper(),
                LastUpdate = deviceInfoRaw.RegistrationDate!.Value,
                TotalRecords = totalItems
            };

            return Ok( new {
                DeviceAddress = deviceAddress,
                Device = deviceInfo,
                Measurement = measurements
            });
            
        }


    }
}