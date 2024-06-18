using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using Infrastructure.Services.SmsService;
using Infrastructure.Services.SmsService.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedModel.Authentication;
using SharedModel.Models;
using SharedModel.System;
using SixLaborsCaptcha.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.AuthService
{
    public class AuthenticationService
    {
        private readonly RedisDataContext _redisDb;
        private readonly MySqlDataContext _mySqlDb;
        private readonly ISixLaborsCaptchaModule _sixLaborsCaptcha;
        private readonly ISmsService _smsService;
        private readonly TemplateService _templateService;

        private readonly string _jwtIssuer;
        private readonly string _jwtKey;

        public AuthenticationService(IConfiguration config, ISixLaborsCaptchaModule sixLaborsCaptchaModule, ISmsService smsService ,TemplateService templateService, MySqlDataContext mySql, RedisDataContext redis)
        {
            _redisDb = redis;
            _mySqlDb = mySql;
            _sixLaborsCaptcha = sixLaborsCaptchaModule;
            _smsService = smsService;
            _templateService = templateService;

            _jwtIssuer = config["Jwt:Issuer"] ?? "";
            _jwtKey = config["Jwt:Key"] ?? "";
        }

        public CaptchaValues GenerateCaptcha()
        {
            try
            {
                var randomNumber = new Random().Next(100000, 999999).ToString();
                var imgText = _sixLaborsCaptcha.Generate(randomNumber);
                var captchaKey = Guid.NewGuid().ToString();

                _redisDb.SetData(captchaKey, randomNumber, 120);

                var captchaValues = new CaptchaValues()
                {
                    CaptchaKey = captchaKey,
                    CaptchaValue = randomNumber.ToString(),
                    ImageText = imgText
                };

                return captchaValues;
            } catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }

        }

        public async Task<string> GenerateOTP(string mobileNumber, string captchaSolve, string captchaKey)
        {
            VerifyCaptcha(captchaSolve, captchaKey);
            IsCustomerExist(mobileNumber);

            try
            {
                var random = new Random();
                var otp = random.Next(10000, 99999).ToString();

                _redisDb.SetData(mobileNumber, otp, 120);

                //await SendOtp(mobileNumber, otp);

                return otp;

            } catch (BoziException ex)
            {
                throw ex;
            }
        }

        private async Task SendOtp(string mobileNumber, string otp)
        {
            try
            {
                var message = _templateService.SMS_loginWithOTP(1, otp, DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss"));
                await _smsService.SendSms(mobileNumber, otp);
            } catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }
        }

        private void VerifyCaptcha(string captchaSolve, string captchaKey)
        {
            var storedCaptchaSolve = _redisDb.GetData<string>(captchaKey) ?? throw new BoziException(400, "کپچا منقضی شده");
            
            if (storedCaptchaSolve != captchaSolve)
            {
                throw new BoziException(400, "کپچا اشتباه است");
            }
        }

        public string VerifyOTP(string mobileNumber, string otp)
        {
            try
            {
                var storedOTP = _redisDb.GetData<string>(mobileNumber);

                if (storedOTP == null)
                {
                    throw new BoziException(400, "کد ورود پیدا نشد");
                }

                if (storedOTP != otp)
                {
                    throw new BoziException(400, "کد ورود اشتباه است");
                }

                var customer = _mySqlDb.CustomerRepository.GetCustomerByMobileNumber(mobileNumber);
                
                if (customer == null || string.IsNullOrEmpty(customer.CustomerID))
                {
                    throw new BoziException(400, "کاربر یافت نشد");
                }

                if (customer.CSID == (int)CustomerStateEnum.Locked)
                {
                    throw new BoziException(401, "حساب شما مسدود شده");
                }

                var token = GenerateToken(customer.CustomerID);

                return token;

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
                  expires: DateTime.Now.AddMinutes(100),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return token;
            }
            catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }
        }

        private bool IsCustomerExist(string mobileNumber)
        {
             var customer = _mySqlDb.CustomerRepository.GetCustomerByMobileNumber(mobileNumber);

             if (customer == null)
             {
                 throw new BoziException(400, "حساب کاربری یافت نشد");
             }

             return true;
        }

    }
}
