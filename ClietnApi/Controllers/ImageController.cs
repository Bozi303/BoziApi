using ClietnApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClietnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {

        private FileManagerClient _client;

        public ImageController(FileManagerClient client)
        {
            _client = client;
        }

        [HttpGet("{title}")]
        public ActionResult<object> View(string title)
        {
            try
            {
                var image =  _client.DownloadFile(title).Result;

                if (image == null || image.Data == null)
                    throw new Exception();

                return File(image.Data, "image/png");
            }
            catch (Exception ex)
            {
                
                return StatusCode(400, ex.Message);
            }
        }
    }
}
