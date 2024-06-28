using ClietnApi.Controllers.PresentationModel;
using ClietnApi.Services;
using Infrastructure.DataAccess.MySql.MySqlModels;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System.Configuration;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StoreController : ControllerBase
    {

        private readonly IBoziService _boziService;
        private readonly FileManagerClient _fileManager;
        private string customerId = "1";


        public StoreController(IBoziService boziService, FileManagerClient fileManager)
        {
            _boziService = boziService;
            _fileManager = fileManager;
        }


        [HttpGet]
        public ActionResult<List<TitleId>> StoreCategories()
        {
            try
            {
                var categories = _boziService.StoreService.GetStoreCategories();

                return categories;

            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }


        [HttpPost]
        public ActionResult RegisterStore(RegisterStoreRequest req)
        {
            try
            {

                var registrationNumber = _boziService.StoreService.GetStoreRegistrationNumber();

                var createStore = new CreateStore
                {
                    CategoryId = req.CategoryId,
                    CityId = req.CityId,
                    CustomerID = customerId,
                    Description = req.Description,
                    Name = req.Title,
                    RegisterNumber = registrationNumber,
                    StatusId = ((int)StoreStatusEnum.Locked).ToString()
                };

                _boziService.StoreService.RegistrationStore(createStore);

                return NoContent();
            } 
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AdRegistration([FromForm] StoreAdRegistrationRequest req)
        {
            try
            {
                //var customerId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                // check customer is store owner


                var PictureIds = StoreFiles(req.Pictures);

                var ad = new CreateAdRequest
                {
                    Title = req.Title,
                    Description = req.Description,
                    Price = req.Price,
                    CustomerId = customerId,
                    StoreId = req.StoreId,
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
            }
            catch
            {
                throw;
            }
        }


    }
}
