using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts.Models;

namespace eMeter.Models.ViewModels
{
    public class AuthenticationViewModel : IUserCredentials
    {

        [Required (ErrorMessage = "El correo electrónico es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Introduzca un correo electrónico válido")]
        [DisplayName("Correo Electrónico")]
        public string? Email {get; set;}
        
        [Required (ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string? Password {get; set;}

        public string? MessageError {get; set;}
    }
}