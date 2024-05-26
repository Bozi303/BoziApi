using Infrastructure.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedModel.System;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AuthService
{
    public class AuthenticationService
    {
        private readonly DataFlowService _db;

        private readonly string _jwtIssuer;
        private readonly string _jwtKey;

        public AuthenticationService(IConfiguration config ,DataFlowService dataFlowService)
        {
            _db = dataFlowService;
            _jwtIssuer = config["Jwt:Issuer"] ?? "";
            _jwtKey = config["Jwt:Key"] ?? "";
        }

        public string GenerateOTP(string mobileNumber)
        {
            try
            {
                var random = new Random();
                var otp = random.Next(10000, 99999).ToString();

                _db.GenerateOTP(mobileNumber, otp);

                return otp;

            } catch (BoziException ex)
            {
                throw ex;
            }
        }

        public string VerifyOTP(string mobileNumber, string otp)
        {
            try
            {
                var storedOTP = _db.VerifyOTP(mobileNumber);
                
                if (storedOTP == null)
                {
                    throw new BoziException(400, "کد ورود پیدا نشد");
                }

                if (storedOTP != otp)
                {
                    throw new BoziException(400, "کد ورود اشتباه است");
                }


                return "";                

            } catch (BoziException ex)
            {
                throw ex;
            }
        }

        private string GenerateToken(string userId)
        {
            try
            {
                var claims = new Claim[]
                {
                    new Claim("UserId", userId)
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(
                  _jwtIssuer,
                  _jwtIssuer,
                  claims,
                  expires: DateTime.Now.AddMinutes(1),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
            }
            catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }
        }
    }
}
