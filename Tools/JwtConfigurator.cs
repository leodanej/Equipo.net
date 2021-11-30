using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


using modelos;
using System;
using System.Collections.Generic;

namespace Tools
{
    public class JwtConfigurator{
        public static string GetToken(usuario userInfo, IConfiguration config ){
            string SecretKey = config["Jwt:Secretkey"];
            string Issuer = config["Jwt:Issuer"];
            string Audience = config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {   
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.correo),
                new Claim("idUsuario", userInfo.Usuarioid.ToString()),
                new Claim("role", userInfo.role.ToString())


            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int GetTokenIdUsuario(ClaimsIdentity identity){
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;

                foreach (var claim in claims)
                {
                    if (claim.Type == "idUsuario")
                    {
                        return int.Parse(claim.Value);
                    }
                }
            }
            return 0;
        }
    }
}