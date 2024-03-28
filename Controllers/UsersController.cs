using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using eMeter.Data;
using eMeter.Models;
using eMeter.Models.ViewModels.Projects;
using eMeterApi.Service;
using eMeterApi.Data.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using eMeterApi.Models.ViewModels.Users;
using eMeterApi.Data.Exceptions;

namespace eMeterSite.Controllers
{
    
    [Auth]
    [Route("[controller]")]
    public class UsersController(ILogger<UsersController> logger, IUserService userService, IProjectService projectService) : Controller
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly IUserService userService = userService;
        private readonly IProjectService projectService = projectService;

        [HttpGet]
        public IActionResult Index()
        {
            var users = Array.Empty<User>();
            try
            {
                var response = userService.GetUsers();

                if( response != null){
                    users = response.ToArray();
                }
            }
            catch (System.Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View( users );

        }
        
        [Route("create")]
        [HttpGet]
        public IActionResult Create(){

            // Get list of projects availables and make a multi select list
            var _projects = this.projectService.GetProjects(null, null)??[];
            var projectsListItems =  _projects.Select( item => new KeyValuePair<string, long>(
                $"{item.Proyecto} ({item.Clave})",  item.Id
            )).ToList();
            ViewData["ProjectsAvailable"] = projectsListItems;

            return View();
        }

        [Route("/")]
        [HttpPost]
        public IActionResult Store( UserRequest userRequest ){

            if (!ModelState.IsValid)
            {
                // Get list of projects availables and make a multi select list
                var _projects = this.projectService.GetProjects(null, null)??[];
                var projectsListItems =  _projects.Select( item => new KeyValuePair<string, long>(
                    $"{item.Proyecto} ({item.Clave})",  item.Id
                )).ToList();
                ViewData["ProjectsAvailable"] = projectsListItems;

                return View("Create", userRequest );
            }

            // Store the user
            try{
                var userId = this.userService.CreateUser( userRequest, null, out string? message );
                if( userId == null){
                    // TODO: Handle error
                    Console.WriteLine("(-) Error message:" + message);
                }
                

            }catch(SimpleValidationException ex){
                
                foreach( var errorProperty in ex.ValidationErrors){
                    ModelState.AddModelError( errorProperty.Key, errorProperty.Value);
                }

                // Get list of projects availables and make a multi select list
                var _projects = this.projectService.GetProjects(null, null)??[];
                var projectsListItems =  _projects.Select( item => new KeyValuePair<string, long>(
                    $"{item.Proyecto} ({item.Clave})",  item.Id
                )).ToList();
                ViewData["ProjectsAvailable"] = projectsListItems;

                return View("Create", userRequest);
            }

            return RedirectToAction("Index", "Users");

        }

        // [Route("{projectId}")]
        // [HttpGet]
        // public async Task<IActionResult> Edit( [FromRoute] int projectId ){
            
        //     try{
        //         var projects = await this.projectService.GetProjects();
        //         if( projects == null){
        //             ViewData["ErrorMessage"] = "Erro al obtener el listado de projectos";
        //             return View("Index", Array.Empty<Project>() );
        //         }
                
        //         var project = projects!.Where( item => item.Id == projectId).FirstOrDefault();
        //         if(project == null){
        //             ViewData["ErrorMessage"] = "El proyecto no se encuentra registrado o esta inactivo.";
        //             return View("Index", projects );
        //         }
                
        //         // Retrive the edit project to edit
        //         var projectViewModel = new NewProjectViewModel{
        //             Proyecto = project.Proyecto,
        //             Clave = project.Clave
        //         };
        //         ViewData["ProjectId"] = project.Id;
        //         return View( projectViewModel );

        //     }catch(Exception err){
        //         ViewData["ErrorMessage"] = err.Message;
        //         return View("Index", Array.Empty<Project>()  );
        //     }
        // }

        // [Route("{projectId}")]
        // [HttpPost]
        // public async Task<IActionResult> Update( NewProjectViewModel newProject, [FromRoute] int projectId ){
            
        //     if (!ModelState.IsValid)
        //     {
        //         return View("Edit", newProject);
        //     }

        //     try{
        //         await this.projectService.UpdateProject( projectId, newProject);
        //     }catch(Exception err){
        //         this._logger.LogError(err, "Error at udate project");
        //     }

        //     return RedirectToAction("Index", "Projects");
        // }


    }
}