using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using eMeter.Data;
using eMeter.Models;
using eMeter.Models.ViewModels.Projects;
using eMeterApi.Service;
using eMeterApi.Data.Contracts;

namespace eMeterSite.Controllers
{
    
    [Auth]
    [Route("[controller]")]
    public class UsersController(ILogger<UsersController> logger, IUserService userService) : Controller
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly IUserService userService = userService;

    
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
        
        // [Route("create")]
        // public IActionResult Create(){
        //     return View();
        // }

        // [Route("store")]
        // [HttpPost]
        // public async Task<IActionResult> Store(NewProjectViewModel newProject){

        //     if (!ModelState.IsValid)
        //     {
        //         return View("Create", newProject); // Pass the model back to the view
        //     }

        //     try{

        //         await this.projectService.CreateProject( newProject );

        //     }catch(ValidationException){
        //         ModelState.AddModelError("Clave", "La clave ya se encuentra almacenada en la base de datos");
        //         return View("Create", newProject);
        //     }

        //     return RedirectToAction("Index", "Projects");

        // }

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