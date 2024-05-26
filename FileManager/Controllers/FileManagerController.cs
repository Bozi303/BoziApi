using FileManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class FileManagerController : ControllerBase
    {

        private readonly IFileManager _fileManager;

        public FileManagerController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpGet]
        public ActionResult<FileModel> GetFile(string fileId)
        {
            try
            {
                var res = _fileManager.GetFile(fileId);

                return Ok(res);

            } catch (FileManagerException ex)
            {
                return StatusCode(ex.Code, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<object> AddFile(IFormFile file)
        {
            try
            {
                byte[] fileData;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileData = ms.ToArray();
                }

                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName).TrimStart('.');

                string fileId = _fileManager.SaveFile(fileData, fileName, fileExtension);

                return Ok(new
                {
                    fileId = fileId,
                });
            }
            catch (FileManagerException ex)
            {
                return StatusCode(ex.Code, ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult DeleteFile(string fileId)
        {
            try
            {
                _fileManager.DeleteFile(fileId);

                return NoContent();
            
            }
            catch (FileManagerException ex)
            {
                return StatusCode(ex.Code, ex.Message);
            }
        }
    }
}
