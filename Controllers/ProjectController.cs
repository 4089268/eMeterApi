using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eMeterApi.Data;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {

        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetProjects()
        {
            var _data = this.projectService.GetAll();
            if(_data == null){
                return Ok( Array.Empty<SysProyecto>() );
            }

            var results = new List<dynamic>();
            foreach( var item in _data){
                results.Add( new {
                    Id = item.Id,
                    Proyecto = item.Proyecto,
                    Clave = item.Clave
                });
            }

            return Ok(results);
        }


        /// <summary>
        /// Store a new entity of SysProyecto at the database
        /// </summary>
        /// <param name="proyecto"></param>
        /// <returns>A entity created</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">The request is not valid</response>
        /// <response code="422">Cant store the entity on the database</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [Produces("application/json")]
        public IActionResult StoreProject( [FromBody] SysProyecto proyecto)
        {

            if(!ModelState.IsValid){
                return BadRequest( ModelState );
            }

            try
            {
                var _newSysProyecto = this.projectService.Create(proyecto);
                return StatusCode( 201, new {id = _newSysProyecto.Id} );
            }
            catch (Exception err)
            {
                return UnprocessableEntity( new { message = $"Cant store the entity; {err.Message}"} );
            }
            
        }
        
        [HttpDelete]
        [Route ("{projectID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult DeleteProject( [FromRoute] long projectID )
        {
            try{
                this.projectService.Remove(projectID);
                return Ok( new {
                    message = $"Project id {projectID} deleted"
                });
            }catch(EntityNotFoundException eNotFound){
                return BadRequest( new {
                    message = eNotFound.Message
                });
            }catch(Microsoft.EntityFrameworkCore.DbUpdateException dbException ){
                return UnprocessableEntity( new {
                    message = dbException.Message
                });
            }
        }

        [HttpPatch]
        [Route ("{projectID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult UpdateProject( [FromRoute] long projectID, [FromBody] SysProyecto sysProyecto )
        {
            try{
                this.projectService.Update(projectID, sysProyecto);
                return Ok( new {
                    message = $"Project id {projectID} deleted"
                });
            }catch(EntityNotFoundException eNotFound){
                return BadRequest( new {
                    message = eNotFound.Message
                });
            }catch(Microsoft.EntityFrameworkCore.DbUpdateException dbException ){
                return UnprocessableEntity( new {
                    message = dbException.Message
                });
            }
        }



    }
}