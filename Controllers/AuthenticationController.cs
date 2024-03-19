using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Data;
using eMeterApi.Helpers;
using eMeterApi.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace eMeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly JwtSettings jwtSettings;

        public AuthenticationController( IOptions<JwtSettings> optionsJwtSettings ){
            this.jwtSettings = optionsJwtSettings.Value;
        } 


        [HttpPost]
        public IActionResult Authenticated( [FromBody] AuthenticationRequest authenticationRequest )
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var jwtGenerator = new JwtGenerator( jwtSettings );

            // TODO: Validate credentials on database

            var token = jwtGenerator.Generate( "1",  authenticationRequest.Email!, "Juan Salvador", null);

            return Ok( new {
                title = "Token generated",
                token
            });
        }
    }
}