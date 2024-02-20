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

        public MeasurementController( IConfiguration c){
            this.configuration = c;
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
                // Return a 400 Bad Request response if validation fails
                return BadRequest( request );
            }

            // Process the data
            var meterData = ProcessBuffer.ProcessData(request);

            // TODO: Store in SQL
            var conectionString = this.configuration.GetConnectionString("eMeter");
            var dbRepository = new Repository(conectionString);
            dbRepository.InsertData( meterData );
            
            return Ok( meterData );
        }
        
    }
}