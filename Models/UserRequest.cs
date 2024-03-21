using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eMeterApi.Data.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace eMeterApi.Models
{
    public class UserRequest : IUser
    {
        [NotMapped]
        [HiddenInput]
        public long Id { get => 0; set => throw new NotImplementedException(); }

        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string? Email {get;set;}
        
        [JsonPropertyName("name")]
        [Required]
        public string? Name {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Required]
        public string? Password {get;set;}

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Required]
        public string? PasswordConfirm {get;set;}

        [JsonPropertyName("company")]
        public string? Company {get;set;}
    }

    public class UserUpdateRequest : IUser
    {
        [NotMapped]
        [HiddenInput]
        public long Id { get => 0; set => throw new NotImplementedException(); }

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