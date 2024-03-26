using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using eMeter.Data;
using eMeter.Models;
using eMeterApi.Data;
using eMeter.Service;

namespace eMeterSite.Controllers
{
    
    [Auth]
    [Route("[controller]")]
    public class DevicesController : Controller
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly EMeterContext eMeterContext;
        private readonly DeviceService deviceService;

        public DevicesController(ILogger<DevicesController> logger, EMeterContext eMeterContext, DeviceService deviceService)
        {
            _logger = logger;
            this.eMeterContext = eMeterContext;
            this.deviceService = deviceService;
        }

        public IActionResult Index([FromQuery] int page = 0, [FromQuery] int chunk = 25, [FromQuery] int PI = 0, [FromQuery] string? SB = null, [FromQuery] string? SV = null ) 
        {

            var devices = this.deviceService.GetDevices(out int totalItems, chunk, page );

            // Process data
            ViewData["ChunkSize"] = chunk;
            ViewData["CurrentPage"] = page;
            ViewData["TotalItems"] = totalItems;

            // TODO: Get catalogs projects
            // var _projects = await appService.GetProjects();
            // if( _projects != null){
            //     ViewData["Projects"] = _projects!;
            // }

            return View( devices );
        }

        [HttpGet]
        [Route("Show/{deviceAddress}")]
        public IActionResult Show([FromRoute] string deviceAddress)
        {
            var deviceDetails = this.deviceService.GetDeviceInfo( deviceAddress );

            if( deviceDetails == null)
            {
                // TODO: Redirect to bad request
                return RedirectToAction("", "Home");
            }

            ViewData["Device"] = deviceDetails!;
            
            return View();
        }
        
    }

}   