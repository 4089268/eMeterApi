using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using eMeter.Models;
using eMeter.Service;
using eMeterApi.Data;
using eMeterApi.Entities;
using eMeterApi.Models;
using eMeterApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {

        private readonly ILogger<MeasurementController> logger;
        private readonly MeasurementService measurementService;
        private readonly DeviceService deviceService;

        public MeasurementController( ILogger<MeasurementController> logger, MeasurementService measurementService, DeviceService deviceService){
            this.logger = logger;
            this.measurementService = measurementService;
            this.deviceService = deviceService;
        }
        

        /// <summary>
        /// Retrive all stored measures
        /// </summary>
        /// <returns> List of  stored measures </returns>
        /// <response code="200">Returns the data</response>
        [HttpGet]
        public IActionResult GetMeasurement( [FromQuery] int chunk = 25, [FromQuery] int page = 0, [FromQuery] string? deviceAddress = "", [FromQuery] string? from = "", [FromQuery] string? to = "" )
        {

            // Validate dates
            if( !DateTime.TryParse(from, out var _from) || !DateTime.TryParse(to, out var _to)){
                return BadRequest( new {
                    title = "Dates are not valid",
                    message = "Invalid date format or values provided for 'from' and 'to'."
                });
            }

            // Get data
            var measurements = this.measurementService.GetMeasurement( _from, _to, out int totalItems, chunk, page, deviceAddress);

            // Return data
            return Ok( new EnumerableResponse<Measurement>(){
                Data = measurements,
                ChunkSize = chunk,
                Page=page,
                TotalItems = totalItems
            }) ;
        }

        [HttpGet]
        [Route("Devices")]
        public IActionResult GetDevices( [FromQuery] int chunk = 25, [FromQuery] int page = 0 )
        {
            var devices = this.deviceService.GetDevices( out int totalItems, chunk, page, null);
            return Ok( new EnumerableResponse<Device>(){
                Data = devices,
                ChunkSize = chunk,
                Page = page,
                TotalItems = totalItems
            });
        }

        [HttpGet]
        [Route("Devices/{deviceAddress}")]
        public ActionResult<DeviceDetails> GetDeviceInfo( [FromRoute] string deviceAddress )
        {
            var deviceInfo = this.deviceService.GetDeviceInfo( deviceAddress );
            return Ok(deviceInfo);
        }
    }
}