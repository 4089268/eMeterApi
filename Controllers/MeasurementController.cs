using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMeterApi.Entities;
using eMeterApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {

        private readonly ILogger<MeasurementController> logger;
        private readonly IConfiguration configuration;
        private readonly EMeterRepository dbRepository;

        public MeasurementController( ILogger<MeasurementController> logger, IConfiguration c, EMeterRepository repository){
            this.logger = logger;
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
        /// <param name="PayloadRequest"> Payload request that hold the digital measure serialized; required; 98 length </param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the payload is not valid </response>
        [HttpPost]
        [Route("/rest/callback/payloads/ul")]
        public IActionResult PostData( DeviceNotification payloadRequest ) {

            //Validate requestBody
            if (string.IsNullOrEmpty( payloadRequest.DataFrame ) || payloadRequest.DataFrame.Length != 98)
            {
                this.logger.LogWarning( "BadRequest at PostData; request={request}", payloadRequest );
                return BadRequest( payloadRequest.DataFrame );
            }

            // Process the data
            var meterData = ProcessBuffer.ProcessData( payloadRequest.DataFrame );

            // Store in database
            dbRepository.InsertData( meterData );
            
            // Return digest model
            return StatusCode( 201, meterData);
        }
        
        /// <summary>
        /// Store a digital measure 
        /// </summary>
        /// <param name="PayloadRequest"> Payload request that hold the digital measure serialized; required; 98 length </param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the payload is not valid </response>
        [HttpGet]
        [Route("/rest/callback/payloads/test")]
        public IActionResult TestPayload( string dataFrame ) {
            
            
            //Validate requestBody
            if (string.IsNullOrEmpty( dataFrame ) || dataFrame.Length != 98)
            {
                this.logger.LogWarning( "BadRequest at PostData; request={request}", dataFrame );
                return BadRequest( dataFrame );
            }

            // Process the data
            var meterData = ProcessBuffer.ProcessData( dataFrame );

            // Return digest model
            return Ok( meterData );
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