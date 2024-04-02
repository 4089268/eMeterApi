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
using Microsoft.AspNetCore.Mvc.TagHelpers;
using eMeterApi.Service;
using eMeterApi.Data.Contracts;

namespace eMeterSite.Controllers
{
    
    [Auth]
    [Route("[controller]")]
    public class DevicesController : Controller
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly DeviceService deviceService;
        private readonly IProjectService projectService;
        
        public DevicesController(ILogger<DevicesController> logger, DeviceService deviceService, IProjectService projectService)
        {
            _logger = logger;
            this.deviceService = deviceService;
            this.projectService = projectService;
        }

        public IActionResult Index([FromQuery] int page = 0, [FromQuery] int chunk = 25, [FromQuery] int PI = 0, [FromQuery] string? SB = null, [FromQuery] string? SV = null ) 
        {
            // Get user id
            long userId = 0;
            try{
                var userIdClaim = HttpContext.User.Claims.Where( item => item.Type == "userId").FirstOrDefault()??throw new Exception("User id not found in the user Claims");
                userId = Convert.ToInt64(userIdClaim!.Value);
            }catch(Exception err){
                this._logger.LogError(err, "Can't obtain the user id at DevicesController.Index");
            }

            // Get projects of the user
            var projects = this.projectService.GetProjects(userId, null)??[];
            var projectsGroupIds = projects.Select( item => item.Clave).ToArray();

            ViewData["Projects"] =  projects;
            
            var devices = this.deviceService.GetDevices(out int totalItems, chunk, page, projectsGroupIds );

            // Process data
            ViewData["ChunkSize"] = chunk;
            ViewData["CurrentPage"] = page;
            ViewData["TotalItems"] = totalItems;
            

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