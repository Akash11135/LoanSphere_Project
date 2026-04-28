using LoanSphere_Frontend.Models;
using LoanSphere_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoanSphereApiClient _apiClient;
        private readonly CurrentUserService _currentUserService;

        public AccountController(LoanSphereApiClient apiClient, CurrentUserService currentUserService)
        {
            _apiClient = apiClient;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_currentUserService.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _apiClient.LoginAsync(model);
            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            _currentUserService.SignIn(result.Data);
            TempData["StatusMessage"] = $"Welcome back, {result.Data.FullName}.";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_currentUserService.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            model.Role = string.IsNullOrWhiteSpace(model.Role) ? "Customer" : model.Role.Trim();

            if (model.Role == "Customer")
            {
                model.SecretKey = string.Empty;
            }

            if ((model.Role == "Admin" || model.Role == "Manager") && string.IsNullOrWhiteSpace(model.SecretKey))
            {
                ModelState.AddModelError(nameof(model.SecretKey), "A secret key is required for admin and manager registration.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _apiClient.RegisterAsync(model);
            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            result.Data.FullName = model.FullName;
            result.Data.Email = model.Email;
            _currentUserService.SignIn(result.Data);
            TempData["StatusMessage"] = "Account created. Complete your profile now or skip and do it later.";

            return RedirectToAction("Edit", "Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _currentUserService.SignOut();
            TempData["StatusMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
