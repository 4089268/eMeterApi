using System;
using System.Collections.Generic;
using System.Linq;
using eMeterApi.Data;
using eMeterApi.Data.Contracts;
using eMeterApi.Helpers;
using eMeterApi.Models.ViewModel;
using eMeterApi.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService userService;

        public AuthenticationController( IUserService userService){
            this.userService = userService;
        } 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationRequest"></param>
        /// <returns></returns>
        /// <response code="200">Authentication valida</response>
        /// <response code="401">Credentials invalid</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult Authenticated( [FromBody] AuthenticationRequest authenticationRequest )
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var token = userService.Authenticate( authenticationRequest, out string? message );
            if( token == null){
                return Unauthorized( new {
                    message = message
                });
            }

            return Ok( new {
                title = "Token generated",
                token
            });
        }
    }
}