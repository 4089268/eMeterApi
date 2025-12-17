using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using eMeterApi.Data.Contracts.Models;

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
namespace eMeter.Models.ViewModels.Projects
{
    public class NewProjectViewModel : IProject
    {
        [Required(ErrorMessage = "El nombre del proyecto es requerido.")]
        public string? Proyecto {get;set;}


        [Required(ErrorMessage = "La clave del proyecto es requerido.")]
        public string? Clave {get;set;}

        [Display(Name = "Oficina")]
        [Required(ErrorMessage = "Seleccione la oficina del proyecto")]
        public int OficinaId {get;set;}
        
        private long _id = 0;
        public long Id { get => _id; set => _id = value; }

        public IEnumerable<SelectListItem> CatalogoOficinas {get;set;} = Array.Empty<SelectListItem>();

    }
}
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).