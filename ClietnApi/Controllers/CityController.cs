using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CityController : ControllerBase
    {

        private readonly IBoziService _boziService;

        public CityController(IBoziService boziService)
        {
            _boziService = boziService;
        }

        [HttpGet]
        public ActionResult<List<Province>> GetProvince()
        {
            try
            {
                var provinces = _boziService.CityService.GetProvinces();
                return provinces;
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<List<City>> GetCitiesByProvinceId(string provinceId)
        {
            try
            {
                var cities = _boziService.CityService.GetCitiesByProvinceId(provinceId);
                return cities;
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
    }
}
