using ClietnApi.Controllers.PresentationModel;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;
using System.Reflection.Metadata;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportController : ControllerBase
    {
        
        private readonly IBoziService _boziSerivce;

        private readonly string customerId = "1";

        public ReportController(IBoziService boziService)
        {
            _boziSerivce = boziService;
        }

        [HttpGet]
        public ActionResult<List<TitleId>> GetReportCategories()
        {          
             try
             {
                 var categories = _boziSerivce.ReportService.GetReportCategories();
                 return categories;
             }
             catch (BoziException ex)
             {
                 return StatusCode(400, ex.Message);
             }         
        }

        [HttpGet]
        public ActionResult<List<TitleId>> GetRerportStatus()
        {
            try
            {
                var status = _boziSerivce.ReportService.GetReportStatus();
                return status;
            } catch (BoziException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ReportAd(ReportAdRequest req)
        {
            try
            {
                var createReport = new CreateCustomerReport
                {
                    AdId = req.AdId,
                    Arcid = req.CategoryId,
                    Arsid = ((int)ReportStatusEnum.NotSeen).ToString(),
                    CustomerId = customerId,
                    Description = req.Description
                };

                _boziSerivce.ReportService.ReportAd(createReport);

                return NoContent();

            } catch (BoziException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
