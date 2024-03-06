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
    public class RestController : ControllerBase
    {

        private readonly ILogger<RestController> logger;
        private readonly EMeterRepository dbRepository;

        public RestController( ILogger<RestController> logger, EMeterRepository repository){
            this.logger = logger;
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
            string? groupId = payloadRequest.Decoded == null ?null :payloadRequest.Decoded.GroupId;
            dbRepository.InsertData( meterData, groupId );
            
            // Return digest model
            return StatusCode( 201, meterData);
        }
        
    }
}