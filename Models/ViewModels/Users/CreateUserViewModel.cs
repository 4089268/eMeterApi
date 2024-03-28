using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eMeterApi.Models.ViewModels.Users
{
    public class CreateUserViewModel
    {

        public UserRequest UserRequest {get;set;}
        public IEnumerable<SelectListItem> ProjectsListItems {get;set;}

        public CreateUserViewModel(){
            UserRequest = new UserRequest();
            ProjectsListItems = Array.Empty<SelectListItem>();
        }

        public CreateUserViewModel( IEnumerable<SelectListItem> projectsListItems ){
            UserRequest = new UserRequest();
            ProjectsListItems = projectsListItems;
        }
        
    }
}