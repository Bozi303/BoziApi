using ClietnApi.Controllers.PresentationModel;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System.Reflection.Metadata;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AdController : ControllerBase
    {
        private readonly IBoziService _boziService;

        private string customerId = "1";

        public AdController(IBoziService boziService)
        {
            _boziService = boziService;
        }

        [HttpGet]
        public ActionResult<List<AdCategory>> Categories(string? parentId)
        {
            try
            {
                var categories = _boziService.AdService.GetAdCategories(parentId ?? "");
                return categories;
            } catch (BoziException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet] 
        public ActionResult<List<string>> MetaDataKeys(string categoryId)
        {
            try
            {
                var metaDataKeys = _boziService.AdService.GetMetaKeysByCategoryId(categoryId);
                return metaDataKeys;
            } catch (BoziException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<List<AdPreview>> GetAdsPreview(AdPreviewRequest req)
        {
            try
            {
                var getAdPreview = new GetAdPreview
                {
                    CategoryId = req.CategoryId,
                    CityIds = req.CityIds,
                    FromDate = req.FromDate,
                    MaxPrice = req.MaxPrice,
                    MinPrice = req.MinPrice,
                    Page = req.Page == 0 ? 1 : req.Page,
                    PageSize = req.PageSize == 0 ? 10 : req.PageSize,
                    ProvinceIds = req.ProvinceIds,
                    Search = req.Search,
                    StatusId = ((int)AdStatusEnum.Accepted).ToString(),
                    ToDate = req.ToDate
                };
                var adPreview = _boziService.AdService.GetAdsPreview(getAdPreview);

                return adPreview;
                
            } catch (BoziException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpGet]
        public ActionResult<Ad> GetAdById(string adId)
        {
            try
            {
                var ad = _boziService.AdService.GetAdById(adId, customerId);
                return ad;
            } catch (BoziException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
