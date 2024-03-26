using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts.Models;

namespace eMeter.Models.ViewModels.Projects
{
    public class NewProjectViewModel : IProject
    {   
        [Required(ErrorMessage = "El nombre del proyecto es requerido.")]
        public string? Proyecto {get;set;}

        [Required(ErrorMessage = "La clave del proyecto es requerido.")]
        public string? Clave {get;set;}
        
        private long _id = 0;
        public long Id { get => _id; set => _id = value; }
    }
}