using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts;
using eMeterApi.Data.Contracts.Models;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using eMeterAPi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            var _users = this.userService.GetUsers();
            if( _users == null){
                return Ok( Array.Empty<IUser>() );
            }

            var results = new List<dynamic>();
            foreach( var user in _users){
                results.Add( new {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Company = user.Company
                });
            }

            return Ok(results);
        }

        [HttpPost]
        public IActionResult CreateUser( [FromBody] UserRequest userRequest)
        {        
            if( !ModelState.IsValid ){
                return BadRequest( ModelState );
            }

            try{
                var userId = this.userService.CreateUser( userRequest, null, out string? message );
                if( userId == null){
                    return UnprocessableEntity( new {
                        title = "Cant store the user",
                        message
                    } );
                }

                return StatusCode( 201,  new {
                    Title = "User created",
                    Id = userId
                });

            }catch(SimpleValidationException validationException){
                return UnprocessableEntity( new {
                    title = "Valiations fail",
                    error= validationException.ValidationErrors
                } );
            }
        }


        [HttpDelete]
        [Route("{userId}")]
        public IActionResult DeleteUser( [FromRoute] long userId )
        {
            try{
                if( this.userService.DisableUser( userId, out string? message) ){
                    return Ok(
                        new {
                            Title = "User has been deleted",
                            Message = "User has been deleted"
                        }
                    );
                }else{
                    return UnprocessableEntity(
                        new {
                            Title = "Can't deleted the user",
                            Message = message
                        }
                    );
                }
            }catch(SimpleValidationException validationException){
                return UnprocessableEntity(
                    new {
                        Title = "Can't deleted the user",
                        Message = validationException.Message,
                        Errors = validationException.ValidationErrors
                    }
                );
            }

        }


        [HttpPatch]
        [Route("{userId}")]
        public IActionResult UpdateUser( [FromRoute] long userId, [FromBody] UserRequest userRequest )
        {
            
            if( !ModelState.IsValid ){
                return BadRequest( ModelState );
            }

            var userParam = new Dictionary<string,object>();
            userParam.Add( "Usuario1", userRequest.Email! );
            userParam.Add( "Operador", userRequest.Name! );
            userParam.Add( "Empresa", userRequest.Company??"" );
            
            // TODO: has password

            try{

                if( this.userService.UpdateUser( userId, userParam , out string? message) ){
                    return Ok(
                        new {
                            Title = "User has been deleted",
                            Message = "User has been deleted"
                        }
                    );
                }else{
                    return UnprocessableEntity(
                        new {
                            Title = "Can't deleted the user",
                            Message = message
                        }
                    );
                }
            }catch(SimpleValidationException validationException){
                return UnprocessableEntity(
                    new {
                        Title = "Can't deleted the user",
                        Message = validationException.Message,
                        Errors = validationException.ValidationErrors
                    }
                );
            }

        }

    }
}