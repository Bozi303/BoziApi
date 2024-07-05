using AdminApi.Controllers.PresentationModels;
using Microsoft.AspNetCore.Mvc;
using SharedModel.BoziService;
using SharedModel.Models;
using SharedModel.System;

namespace AdminApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StoreController : ControllerBase
    {
        
        private readonly IBoziService _boziService;

        private readonly string adminId = "1";

        public StoreController(IBoziService boziSerivce)
        {
            _boziService = boziSerivce;
        }

        [HttpGet]
        public ActionResult<List<TitleId>> StoreStatuses()
        {
            try
            {
                var statuses = _boziService.StoreService.GetStoreStatuses();
                return statuses;
                
            } catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
        
        [HttpGet]
        public ActionResult VerifyStore(VerifyStore req)
        {
            try
            {
                var changeAdStatus = new ChangeStoreStatus
                {
                    StatusId = ((int)StoreStatusEnum.Accepted).ToString(),
                    StoreId = req.StoreId,
                    Note = ""
                };

                _boziService.StoreService.ChangeStoreStatus(changeAdStatus, adminId);

                return NoContent();
            }
            catch (BoziException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);
            }
        }
    }
}
