using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class LoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
