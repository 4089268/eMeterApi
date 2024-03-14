using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace eMeterApi.Helpers
{
    public class JwtGenerator
    {

        private readonly byte[] TokenKey;
        private readonly TimeSpan TokenLifeTime;

        public JwtGenerator(string key){
            this.TokenKey =  Encoding.UTF8.GetBytes(key);
            TokenLifeTime = TimeSpan.FromDays(5);
        }

        public string Generate( string email, string name, string userId ){
            
            var claims = new List<Claim>{
                new( JwtRegisteredClaimNames.Email, email),
                new( JwtRegisteredClaimNames.Name, name),
                new( "userId", userId)
            };

            var tokenDescription = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity( claims),
                Expires = DateTime.UtcNow.Add( TokenLifeTime),
                Issuer = "https://emeter.arquos.ddns.net",
                Audience = "https://emeter.arquos.ddns.net",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey( TokenKey),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken( tokenDescription );
            var jwt = tokenHandler.WriteToken( token );
            return jwt;
        }

    }
}