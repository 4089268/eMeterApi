using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMeterApi.Data
{
    public class SaasSettings
    {
        public string Endpoint {get;set;} = null!;
        public string User {get;set;} = null!;
        public string Password {get;set;} = null!;
    }
}