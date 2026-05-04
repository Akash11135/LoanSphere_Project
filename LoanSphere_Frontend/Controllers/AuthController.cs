using LoanSphere_Frontend.DTOs;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LoanSphere_Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;
        public AuthController(HttpClient http)
        {
            _http = http;

        }

     

        public IActionResult Logout()
        {
            
            Response.Cookies.Delete("jwt");

            TempData["Success"] = "Logged out successfully";

            return RedirectToAction("Login", "Auth"); 
        }

        public IActionResult Login()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginDto = new LoginDto()
            {
                Email = email,
                Password = password
            };

            var resp = await _http.PostAsJsonAsync(
                "http://localhost:5000/api/auth/login",
                loginDto);

            var json = await resp.Content.ReadAsStringAsync();
            Console.WriteLine(json); 

            if (resp.IsSuccessStatusCode)
            {
                var result = await resp.Content.ReadFromJsonAsync<Loginresponse>();
               
                if (!string.IsNullOrEmpty(result?.token))
                {
                    
                    Response.Cookies.Append("jwt", result.token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // IMPORTANT for localhost HTTP
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddHours(2)
                    });

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(result.token);
                    var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "Borrower";

                    if (role == "Admin")
                    {
                        return RedirectToAction("Index","Admin");
                    }

                    return RedirectToAction("Index", "Dashboard");
                }
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