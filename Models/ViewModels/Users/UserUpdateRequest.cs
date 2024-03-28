using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eMeterApi.Models.ViewModels.Users
{
    public class UserUpdateRequest
    {
        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress)]
        public string? Email {get;set;}
        
        [JsonPropertyName("name")]
        public string? Name {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        public string? Password {get;set;}

        [JsonPropertyName("passwordConfirm")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? PasswordConfirm {get;set;}

        [JsonPropertyName("company")]
        public string? Company {get;set;}
    }
}