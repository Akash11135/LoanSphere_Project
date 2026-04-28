using LoanSphere_Frontend.Models;
using LoanSphere_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CurrentUserService _currentUserService;
        private readonly LoanSphereApiClient _apiClient;

        public ProfileController(CurrentUserService currentUserService, LoanSphereApiClient apiClient)
        {
            _currentUserService = currentUserService;
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _apiClient.GetProfileAsync(user.UserId, user.Token);
            if (!result.Success || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "Dashboard");
            }

            var viewModel = new ProfileEditViewModel
            {
                FullName = result.Data.FullName,
                Email = result.Data.Email,
                Phone = result.Data.Phone,
                Role = result.Data.Role,
                ProfilePictureUrl = result.Data.ProfilePictureUrl,
                PanCardNumber = result.Data.PanCardNumber,
                AadhaarNumber = result.Data.AadhaarNumber,
                CibilScore = result.Data.CibilScore,
                IsProfileComplete = result.Data.IsProfileComplete
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _apiClient.UpdateProfileAsync(user.UserId, model, user.Token);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["StatusMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Skip()
        {
            TempData["StatusMessage"] = "You can complete your profile later from the dashboard.";
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
