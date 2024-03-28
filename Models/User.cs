using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eMeter.Models
{
    public class User
    {
        public long Id {get;set;}
        
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;} = "";
        
        public string Name {get;set;} = "";
        
        public string Company {get;set;} = "";

        public IEnumerable<Project>? Projects {get;set;}
    }
}