using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace eMeterSite.Views.Shared
{
    public class Paginator
    {
        public int TotalItems {get;set;} = 0;
        public string Route {get;set;} = "/";
        public string[] Params {get;set;} = null!;
        public int CurrentPage {get;set;} = 0;
        public int ChunkSize {get;set;} = 25;
        
    }
}