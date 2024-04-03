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
using Microsoft.AspNetCore.Http.Features;
using eMeterApi.Models.ViewModels.Devices;

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

        public IActionResult Index( DeviceIndexFilterViewModel filter) 
        {

            // Get user id
            long userId = 0;
            string userName = "";
            try{
                var userIdClaim = HttpContext.User.Claims.Where( item => item.Type == "userId").FirstOrDefault()??throw new Exception("User id not found in the user Claims");
                userId = Convert.ToInt64(userIdClaim!.Value);
                
                // TODO: Replace for some user role
                userName = HttpContext.User.Claims.Where( claim => claim.Type == "name" ).FirstOrDefault()!.Value;

            }catch(Exception err){
                this._logger.LogError(err, "Can't obtain the user id at DevicesController.Index");
            }

            // Get projects of the user
            var projects = this.projectService.GetProjects(userId, null)??[];
            var _projectsKey = projects.Select( item => item.Clave).ToArray();

            // Get the devices asigned to the user
            var devices = this.deviceService.GetDevices(out int totalItems, filter.Chunk, filter.Page,
                groupsId: string.IsNullOrEmpty(filter.PK)
                    ? (userName.ToLower().Equals("administrador") ?null :_projectsKey)
                    :[filter.PK],
                batteryStatus: string.IsNullOrEmpty(filter.BS) ?null :[filter.BS],
                valveStatus:  string.IsNullOrEmpty(filter.VS) ?null : [filter.VS],
                search: filter.S
            );

            // Process view data
            ViewData["TotalItems"] = totalItems;

            // Create catalogs for the filters
            var _listItemsProjects = new List<SelectListItem>(){ new ("Todos", "")};
            _listItemsProjects.AddRange( projects.Select( p => new SelectListItem( $"{p.Proyecto} - {p.Clave}", p.Clave)) );
            ViewData["Projects"] =_listItemsProjects;
            ViewData["ValveStatuses"] = new List<SelectListItem>(){
                new("Todos", ""),
                new("Cerrado", "closed"),
                new("Abierto", "open"),
                new("Otro", "otro")
            };
            ViewData["BatteryStatuses"] = new List<SelectListItem>(){
                new("Todos", ""),
                new("Bateria Baja", "low battery"),
                new("Normal", "normal"),
                new("Otro", "otro")
            };

            var indexViewModel = new DevicesIndexViewModel(devices){ Filter = filter };
            return View( indexViewModel );
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

            // Get char values
            var chartData = this.deviceService.GetMeasurementChart(deviceAddress);
            ViewData["ChartData"] = chartData;
            
            return View();
        }
        
    }

}   