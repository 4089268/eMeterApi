using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMeterApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly EMeterRepository dbRepository;

        public MeasurementController( IConfiguration c, EMeterRepository repository){
            this.configuration = c;
            this.dbRepository = repository;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index() {
            return Ok( "eMeter 1.1" );
        }


        [HttpPost]
        public IActionResult PostData( string request) {

            //Validate requestBody
            if (string.IsNullOrEmpty(request) || request.Length != 98)
            {
                return BadRequest( request );
            }

            // Process the data
            var meterData = ProcessBuffer.ProcessData(request);

            // Store in database
            var conectionString = this.configuration.GetConnectionString("eMeter");
            dbRepository.InsertData( meterData );
            
            // Return digest model
            return Ok( meterData );
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<MeterData>> GetData()
        {
            var data = dbRepository.GetAll().ToList();
            return data;
        }

    }
}