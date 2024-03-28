using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eMeterApi.Models.ViewModels.Users
{
    public class UserRequest
    {
        
        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ser un correo electrónico válido")]
        [Required (ErrorMessage = "Correo electrónico requerido")]
        [DisplayName("Correo Electrónico")]
        public string? Email {get;set;}
        
        [JsonPropertyName("name")]
        [Required (ErrorMessage = "El nombre del usuario es requerido")]
        [DisplayName("Nombre del operador")]
        public string? Name {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Required ( ErrorMessage = "La contraseña es requerida")]
        [DisplayName("Contraseña")]
        public string? Password {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden, revise e intente de nuevo.")]
        [Required( ErrorMessage = "La contraseña de confirmación es requerida.") ]
        [DisplayName("Confirmar contraseña")]
        public string? PasswordConfirm {get;set;}

        [JsonPropertyName("company")]
        [DisplayName("Empresa")]
        public string? Company {get;set;}

        [DisplayName("Proyectos")]
        public IEnumerable<long> ProjectsId {get;set;} = [];

    }

}