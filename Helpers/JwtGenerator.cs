using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eMeterApi.Data;
using Microsoft.IdentityModel.Tokens;

namespace eMeterApi.Helpers
{
    public class JwtGenerator
    {

        private readonly JwtSettings jwtSettings;
        private readonly TimeSpan TokenLifeTime;

        public JwtGenerator( JwtSettings settings){
            this.jwtSettings = settings;
            TokenLifeTime = TimeSpan.FromDays(5);
        }

        public string Generate( string userId, string email, string name, IDictionary<string,string>? customClaims ){
            
            // Add basic claims
            var claims = new List<Claim>{
                new( "userId", userId),
                new( JwtRegisteredClaimNames.Email, email),
                new( JwtRegisteredClaimNames.Name, name),
            };

            // Add custom claims
            if( customClaims != null ){
                foreach( var keyValuePair in customClaims){
                    claims.Add( new Claim( keyValuePair.Key.ToString(), keyValuePair.Value.ToString() ));
                }
            }
            
            
            var tokenDescription = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity( claims),
                Expires = DateTime.UtcNow.Add( TokenLifeTime),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey( Encoding.UTF8.GetBytes(jwtSettings.Key!) ),
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