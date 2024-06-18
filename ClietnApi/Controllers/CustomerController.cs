using ClietnApi.Controllers.PresentationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]/[action]")]
    public class CustomerController : ControllerBase
    {

        private readonly IBoziService _boziService;

        private readonly string CustomerId = "1";

        public CustomerController(IBoziService boziService)
        {
            _boziService = boziService;
        }


        [HttpGet]
        public ActionResult<CustomerProfile> GetProfile()
        {
            try
            {
                var customerId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                var customer = _boziService.CustomerService.GetCustomerProfile(customerId ?? "");

                return customer;

            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AdRegistration([FromBody] AdRegistrationRequest req)
        {
            try
            {
                var ad = new CreateAdRequest
                {
                    Title = req.Title,
                    Description = req.Description,
                    Price = req.Price,
                    CustomerId = CustomerId,
                    StoreId = "-1",
                    AdCategoryId = req.AdCategoryId,
                    CityId = req.CityId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    StatusId = (int)AdStatusEnum.Pending
                };
                _boziService.CustomerService.AdRegistration(ad);
                return Ok();
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

    }
}
