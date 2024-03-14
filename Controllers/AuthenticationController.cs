using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Data;
using eMeterApi.Helpers;
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
        public IActionResult Authenticated( [FromBody] string user, [FromBody] string password )
        {
            var jwtGenerator = new JwtGenerator( jwtSettings.Value.Key );

            var token = jwtGenerator.Generate(user, "user", "1");

            return Ok( new {
                title = "Token generated",
                token = token
            });
        }
    }
}