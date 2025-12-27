using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using eMeter.Models;
using eMeterApi.Data.Contracts;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using eMeterApi.Models;
using eMeter.Service;

namespace eMeterApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> logger;
        private readonly IProjectService projectService;
        private readonly DeviceService deviceService;

        public ProjectsController(ILogger<ProjectsController> logger, IProjectService projectService, DeviceService deviceServ)
        {
            this.logger = logger;
            this.projectService = projectService;
            this.deviceService = deviceServ;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        public ActionResult<BasicResponse<IEnumerable<ProjectResponse>>> GetProjects()
        {
            var projectsResponse = new List<ProjectResponse>();
            try
            {
                // Retrive the projects
                IEnumerable<Project> projects = this.projectService.GetProjects(null, null) ?? Array.Empty<Project>();

                // Retrive the devices
                List<Device> devices = this.deviceService.GetDevices(out int _totalDevices, chunk: 0).ToList(); // Retrive all

                foreach(var p in projects)
                {
                    var _devices = devices.Where(d => d.GroupId == p.Clave).ToList();
                    devices.RemoveAll(d => d.GroupId == p.Clave);

                    var _projResponse = new ProjectResponse
                    {
                        Id = p.Id,
                        Proyecto = p.Proyecto,
                        Clave = p.Clave,
                        OficinaId = p.OficinaId,
                        OficinaDesc = p.OficinaDesc,
                        Devices = _devices,
                        TotalDevices = _devices.Count()
                    };
                    projectsResponse.Add(_projResponse);
                }

                if(devices.Any())
                {
                    var _projResponse = new ProjectResponse
                    {
                        Id = 0,
                        Proyecto = "-- SIN PROYECTO --",
                        Clave = "",
                        OficinaId = 0,
                        OficinaDesc = "",
                        Devices = devices,
                        TotalDevices = devices.Count()
                    };
                    projectsResponse.Add(_projResponse);
                }

                return Ok(new BasicResponse<IEnumerable<ProjectResponse>>
                {
                    Title = "Proyectos obtenidos",
                    Value = projectsResponse
                });
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error al obtener los proyectos registrados.");
                return Conflict(new BasicResponse<IEnumerable<ProjectResponse>>
                {
                    Title = "Error al obtener los datos",
                    Message = ex.Message
                });
            }
        }

    }
}