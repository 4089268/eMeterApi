#pragma warning disable CS8766 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using eMeterApi.Data.Contracts.Models;

namespace eMeterApi.Models.ViewModel
{
    public class AuthenticationRequest : IUserCredentials
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [JsonPropertyName("email")]
        public string? Email {get;set;}


        [Required]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string? Password {get;set;}

    }
}