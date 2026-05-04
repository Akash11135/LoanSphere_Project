using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Unauthorised()
        {
            return View();
        }
        public IActionResult Nodata()
        {
            return View();
        }
    }
}
