using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using eMeter.Data;
using eMeterApi.Data.Contracts;
using eMeter.Models;
using eMeter.Models.ViewModels.Projects;
using eMeterApi.Data.Contracts.Models;
using eMeterApi.Data;

namespace eMeterSite.Controllers
{
    
    [Auth]
    [Route("[controller]")]
    public class ProjectsController(ILogger<ProjectsController> logger, IProjectService projectService, EMeterContext context ) : Controller
    {
        private readonly ILogger<ProjectsController> _logger = logger;
        private readonly IProjectService projectService = projectService;

        private readonly EMeterContext eMeterContext = context;
    
        public IActionResult Index()
        {
            var projects = Array.Empty<Project>();
            try
            {
                var response = projectService.GetProjects(null, null);
                if( response != null)
                {
                    projects = response.ToArray();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //TODO: Handle the error
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View( projects );

        }
        
        [Route("create")]
        public IActionResult Create()
        {
            var viewModel = new NewProjectViewModel();
            viewModel.CatalogoOficinas = this.eMeterContext.CatOficinas.Where(of => of.Inactivo == false).Select(of => new SelectListItem
            {
                Value = of.Id.ToString(),
                Text = of.Oficina
            }).ToArray();
            return View(viewModel);
        }

        [Route("store")]
        [HttpPost]
        public IActionResult Store(NewProjectViewModel newProject)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", newProject); // Pass the model back to the view
            }

            try
            {

                var project = new Project
                {
                    Proyecto = newProject.Proyecto ?? "",
                    Clave = newProject.Clave ?? "",
                    OficinaId = newProject.OficinaId
                };

                var projectId = this.projectService.CreateProject(project, out string? message);
                if( message != null)
                {
                    _logger.LogWarning(message);
                }

            }
            catch(ValidationException)
            {
                ModelState.AddModelError("Clave", "La clave ya se encuentra almacenada en la base de datos");
                return View("Create", newProject);
            }

            return RedirectToAction("Index", "Projects");

        }

        [Route("{projectId}")]
        [HttpGet]
        public IActionResult Edit([FromRoute] int projectId )
        {
            try
            {
                var projects = this.projectService.GetProjects(null, null);
                if(projects == null)
                {
                    ViewData["ErrorMessage"] = "Erro al obtener el listado de projectos";
                    return View("Index", Array.Empty<Project>() );
                }
                
                var project = projects!.Where( item => item.Id == projectId).FirstOrDefault();
                if(project == null)
                {
                    ViewData["ErrorMessage"] = "El proyecto no se encuentra registrado o esta inactivo.";
                    return View("Index", projects );
                }
                
                // Retrive the edit project to edit
                var projectViewModel = new NewProjectViewModel
                {
                    Proyecto = project.Proyecto,
                    Clave = project.Clave,
                    OficinaId = project.OficinaId
                };

                // Obtener catalogo oficinas
                projectViewModel.CatalogoOficinas = this.eMeterContext.CatOficinas.Where(of => of.Inactivo == false).Select(of => new SelectListItem
                {
                    Value = of.Id.ToString(),
                    Text = of.Oficina
                }).ToArray();

                ViewData["ProjectId"] = project.Id;
                return View(projectViewModel);
            }
            catch(Exception err)
            {
                ViewData["ErrorMessage"] = err.Message;
                return View("Index", Array.Empty<Project>());
            }
        }

        [Route("{projectId}")]
        [HttpPost]
        public IActionResult Update(NewProjectViewModel newProject, [FromRoute] int projectId)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", newProject);
            }

            try
            {
                var project = new Project
                {
                    Proyecto = newProject.Proyecto ?? "",
                    Clave = newProject.Clave ?? "",
                    OficinaId = newProject.OficinaId
                };

                this.projectService.UpdateProject(projectId, project, out string? message);
                if( message != null)
                {
                    this._logger.LogWarning( message );
                }
            }
            catch(Exception err)
            {
                this._logger.LogError(err, "Error at udate project");
            }

            return RedirectToAction("Index", "Projects");
        }

    }
}