using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
       
    }
}