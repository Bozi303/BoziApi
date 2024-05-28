using ClietnApi.Controllers.PresentationModel;
using Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using SixLaborsCaptcha.Core;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly AuthenticationService _authService;
        private readonly IBoziService _boziService;

        public AuthenticationController(AuthenticationService authService, IBoziService boziSerivce)
        {
            _authService = authService;
            _boziService = boziSerivce;
        }

        [HttpGet]
        public ActionResult<CaptchaResponse> GetCaptchaImage()
        {
            try
            {
                var res = _authService.GenerateCaptcha();

                return new CaptchaResponse
                {
                    CaptchaImage = res.ImageText,
                    CaptchaKey = res.CaptchaKey
                };

            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<SendOtpResponse> SendOtp(SendOtpRequest req)
        {
            try
            {
                var otp = _authService.GenerateOTP(req.MobileNumber, req.CaptchaSolve, req.CaptchaKey).Result;

                return new SendOtpResponse
                {
                    Otp = otp,
                };
            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<VerifyOtpResponse> VerifyOtp(VerifyOtpRequest req)
        {
            try
            {
                var token = _authService.VerifyOTP(req.MobileNumber, req.Otp);

                return new VerifyOtpResponse
                {
                    token = token
                };

            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult<CustomerProfile> GetProfile()
        {
            try
            {
                var customerId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                var customer = _boziService.CustomerService.GetCustomerProfile(customerId);

                return customer;

            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
    }
}
