using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eMeterApi.Data.Contracts;
using eMeter.Models.ViewModels;

namespace eMeterApi.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> logger;
        private readonly IUserService userService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login( AuthenticationViewModel authenticationViewModel)
        {

            var token = userService.Authenticate( authenticationViewModel, out string? message );

            if( token == null){
                authenticationViewModel.MessageError = message;
                ViewData["ErrorMessage"] = "Usuario y/o contraseña incorrectos.";
                return View("index", authenticationViewModel);
            }
            
            // Store token securely, e.g., in session
            HttpContext.Session.SetString( "JWTToken", token);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}