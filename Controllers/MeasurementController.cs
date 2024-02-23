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


        /// <summary>
        /// Store a digital measure
        /// </summary>
        /// <param name="payload"> Digital measure serialized; required; 98 length </param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the payload is not valid </response>
        [HttpPost]
        public IActionResult PostData( string payload) {

            //Validate requestBody
            if (string.IsNullOrEmpty( payload) || payload.Length != 98)
            {
                return BadRequest( payload );
            }

            // Process the data
            var meterData = ProcessBuffer.ProcessData( payload );

            // Store in database
            var conectionString = this.configuration.GetConnectionString("eMeter");
            dbRepository.InsertData( meterData );
            
            // Return digest model
            return StatusCode( 201, meterData);
        }
        
        [HttpPost]
        [Route("/res/callback/payloads/ul")]
        public IActionResult PostData( PayloadRequest payloadRequest ) {

            //Validate requestBody
            if (string.IsNullOrEmpty( payloadRequest.Payload) || payloadRequest.Payload.Length != 98)
            {
                return BadRequest( payloadRequest.Payload );
            }

            // Process the data
            var meterData = ProcessBuffer.ProcessData( payloadRequest.Payload );

            // Store in database
            var conectionString = this.configuration.GetConnectionString("eMeter");
            dbRepository.InsertData( meterData );
            
            // Return digest model
            return StatusCode( 201, meterData);
        }
        

        /// <summary>
        /// Retrive all stored measures
        /// </summary>
        /// <returns> List of  stored measures </returns>
        /// <response code="200">Returns the data</response>
        [HttpGet]
        public ActionResult<IEnumerable<MeterData>> GetData()
        {
            var data = dbRepository.GetAll().ToList();
            return data;
        }

    }
}