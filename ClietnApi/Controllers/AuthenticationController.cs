using Microsoft.AspNetCore.Mvc;

namespace ClietnApi.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
