using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMeterApi.Data
{
    public class JwtSettings
    {
        public string Key {get;set;} = null!;
        public string Audience {get;set;} = null!;
        public string Issuer {get;set;} = null!;
    }
}