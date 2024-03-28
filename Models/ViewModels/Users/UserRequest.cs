using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Models.ViewModels.Users
{
    public class UserRequest
    {
        
        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ser un correo electrónico válido")]
        [Required (ErrorMessage = "Correo electrónico requerido")]
        public string? Email {get;set;}
        
        [JsonPropertyName("name")]
        [Required (ErrorMessage = "El nombre del usuario es requerido")]
        public string? Name {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Required ( ErrorMessage = "La contraseña es requerida")]
        public string? Password {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden, revise e intente de nuevo.")]
        [Required( ErrorMessage = "La contraseña de confirmación es requerida.") ]
        public string? PasswordConfirm {get;set;}

        [JsonPropertyName("company")]
        public string? Company {get;set;}

        public IEnumerable<int>? ProjectsId {get;set;}
    }

}