using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedModel.System;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AuthService
{
    public class JwtService
    {
        private readonly string _issuer;
        private readonly string _key;
        public JwtService(IConfiguration config)
        {
            _issuer = config["Jwt:Issuer"] ?? "";
            _key = config["Jwt:Key"] ?? "";
        }

        public string GenerateToken(string userId)
        {
            try
            {
                var claims = new Claim[]
                {
                    new Claim("UserId", userId)
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(
                  _issuer,
                  _issuer,
                  claims,
                  expires: DateTime.Now.AddMinutes(1),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
            } catch (Exception ex) 
            {
                throw new BoziException(500, ex.Message);
            }
        }
    }
}
