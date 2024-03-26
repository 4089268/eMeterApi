using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts.Models;

namespace eMeter.Models.ViewModels
{
    public class AuthenticationViewModel : IUserCredentials
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email {get; set;}
        
        [Required]
        [DataType(DataType.Password)]
        public string? Password {get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string? MessageError {get; set;}
        
    }
}