using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMeterApi.Models;
using eMeterApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {

        private readonly ILogger<MeasurementController> logger;
        private readonly EMeterRepository dbRepository;

        public MeasurementController( ILogger<MeasurementController> logger, EMeterRepository repository){
            this.logger = logger;
            this.dbRepository = repository;
        }
        

        /// <summary>
        /// Retrive all stored measures
        /// </summary>
        /// <returns> List of  stored measures </returns>
        /// <response code="200">Returns the data</response>
        [HttpGet]
        public ActionResult<IEnumerable<MeterData>> GetMeasurement()
        {
            var data = dbRepository.GetAll().ToList();
            return data;
        }

    }
}