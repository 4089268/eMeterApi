using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eMeterApi.Models.ViewModels.Users
{
    public class UserEditRequest
    {
        public long UserId { get; set; } = 0;

        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ser un correo electrónico válido")]
        [Required(ErrorMessage = "Correo electrónico requerido")]
        [DisplayName("Correo Electrónico")]
        public string? Email { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "El nombre del usuario es requerido")]
        [DisplayName("Nombre del operador")]
        public string? Name { get; set; }

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres")]
        [DisplayName("Contraseña")]
        public string? Password { get; set; }

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden, revise e intente de nuevo.")]
        [DisplayName("Confirmar contraseña")]
        public string? PasswordConfirm { get; set; }

        
        [DisplayName("Empresa")]
        public string? Company {get;set;}

        [DisplayName("Proyectos")]
        public List<long> ProjectsId { get; set; } = new List<long>();
    }
}