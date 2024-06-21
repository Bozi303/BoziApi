using ClietnApi.Controllers.PresentationModel;
using ClietnApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;

namespace ClietnApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("/api/[controller]/[action]")]
    public class CustomerController : ControllerBase
    {

        private readonly IBoziService _boziService;
        private readonly FileManagerClient _fileManager;

        private readonly string customerId = "1";

        public CustomerController(IBoziService boziService, FileManagerClient fileManager)
        {
            _boziService = boziService;
            _fileManager = fileManager;
        }


        [HttpGet]
        public ActionResult<CustomerProfile> GetProfile()
        {
            try
            {
                //var customerId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                var customer = _boziService.CustomerService.GetCustomerProfile(customerId ?? "");

                return customer;

            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        
        [HttpPut]
        public ActionResult EditProfile(EditProfileRequest req)
        {
            try
            {
                //var customerId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                var customerProfile = new EditCustomerProfile
                {
                    CityId = req.CityId,
                    CustomerId = customerId,
                    Email = req.Email,
                    FirstName = req.FirstName,
                    LastName = req.LastName
                };

                _boziService.CustomerService.UpdateCustomerProfile(customerProfile);

                return NoContent();
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
        

        [HttpPost]
        public ActionResult AdRegistration([FromForm] AdRegistrationRequest req)
        {
            try
            {
                //var customerId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                var PictureIds = StoreFiles(req.Pictures); 

                var ad = new CreateAdRequest
                {
                    Title = req.Title,
                    Description = req.Description,
                    Price = req.Price,
                    CustomerId = customerId,
                    StoreId = "-1",
                    AdCategoryId = req.AdCategoryId,
                    CityId = req.CityId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    StatusId = (int)AdStatusEnum.Pending,
                    PicutresIds = PictureIds,
                    MetaDatas = req.MetaDatas
                };

                _boziService.CustomerService.AdRegistration(ad);
                return Ok();
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }


        private List<string> StoreFiles(List<IFormFile> files)
        {
            try
            {
                List<string> Ids = new();

                foreach (var file in files)
                {
                    var id = _fileManager.UploadFile(file).Result;
                    Ids.Add(id);
                }

                return Ids;
            } catch
            {
                throw;
            }
        }

    }
}
