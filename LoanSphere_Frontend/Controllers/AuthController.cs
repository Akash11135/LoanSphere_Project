using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class AuthController : Controller
    {
        // GET Login
        public IActionResult Login()
        {
            return View();
        }

        // POST Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Dummy credentials
            if (email == "akash@gmail.com" && password == "akash123")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Wrong email or password";
            return View();
        }

        // Register Page
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
            string fullname,
            string email,
            string password)
        {
            ViewBag.Success =
                "Registration successful (dummy only). Please login.";

            return View();
        }
    }
}