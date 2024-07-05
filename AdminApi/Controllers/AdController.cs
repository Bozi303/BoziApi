using AdminApi.Controllers.PresentationModels;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdController : ControllerBase
    {

        private readonly IBoziService _boziService;
        private readonly string adminId = "1";

        public AdController(IBoziService boziService)
        {
            _boziService = boziService;
        }

        [HttpGet]
        public ActionResult<List<TitleId>> AdStatuses()
        {
            try
            {
                var statuses = _boziService.AdService.GetAdStatuses();
                return statuses;
            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
      
        [HttpPost]
        public ActionResult VerifyAd(VerifyAd req)
        {
            try 
            {
                var changeAdStatus = new ChangeAdStatus
                {
                    StatusId = ((int)AdStatusEnum.Accepted).ToString(),
                    AdId = req.AdId,
                    Note = ""
                };

                _boziService.AdService.ChangeAdStatus(changeAdStatus, adminId);

                return NoContent();
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult RejectAd(RejectAd req)
        {
            try
            {
                var changeAdStatus = new ChangeAdStatus
                {
                    StatusId = ((int)AdStatusEnum.Rejected).ToString(),
                    AdId = req.AdId,
                    Note = req.Note
                };

                _boziService.AdService.ChangeAdStatus(changeAdStatus, adminId);

                return NoContent();
            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }

    }
}
