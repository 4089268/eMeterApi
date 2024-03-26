using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eMeterApi.Data;
using eMeter.Service;
using eMeter.Data;
using eMeter.Models;
using eMeter.Models.ViewModels;

namespace eMeterApi.Controllers
{
    
    [Auth]
    [Route("[controller]")]
    public class MeasurementController : Controller
    {
        private readonly MeasurementService measurementService;
        private readonly ILogger<MeasurementController> _logger;

        public MeasurementController(ILogger<MeasurementController> logger, MeasurementService measurementService)
        {
            _logger = logger;
            this.measurementService = measurementService;
        }

        public IActionResult Index( [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, [FromQuery] string? deviceAddress = null  ) 
        {
            ViewData["Title"] = "Measurement";
            
            MeasurementViewModel measurementViewModel = new();

            if( desde != null && hasta != null){
                measurementViewModel.Desde = desde.Value;
                measurementViewModel.Hasta = hasta.Value;
            }else{
                measurementViewModel.Desde = DateTime.Now;
                measurementViewModel.Hasta = DateTime.Now;
            }

            var measurements = this.measurementService.GetMeasurement( measurementViewModel.Desde, measurementViewModel.Hasta, out int totalItems, deviceAddress: deviceAddress );

            // Prepare data to view
            ViewData["TotalItems"] = totalItems;
            ViewData["Measurements"] = measurements;

            return View( measurementViewModel );
        }

        
    }
}   