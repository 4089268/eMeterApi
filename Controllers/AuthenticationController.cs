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

        private readonly IOptions<JwtSettings> jwtSettings;

        public AuthenticationController( IOptions<JwtSettings> jwtSettings ){
            this.jwtSettings = jwtSettings;
        } 


        [HttpPost]
        [Route("/")]
        public IActionResult Authenticated( [FromBody] AuthenticationRequest authenticationRequest )
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var jwtGenerator = new JwtGenerator( jwtSettings.Value.Key );

            // TODO: Validate credentials on database

            var token = jwtGenerator.Generate( authenticationRequest.Email!, "Juan Salvador", "1");

            return Ok( new {
                title = "Token generated",
                token = token
            });
        }
    }
}