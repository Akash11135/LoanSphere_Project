using LoanSphere_Frontend.Models;
using LoanSphere_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class LoanController : Controller
    {
        private readonly CurrentUserService _currentUserService;
        private readonly LoanSphereApiClient _apiClient;

        public LoanController(CurrentUserService currentUserService, LoanSphereApiClient apiClient)
        {
            _currentUserService = currentUserService;
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                TempData["ErrorMessage"] = "Please log in to apply for a loan.";
                return RedirectToAction("Login", "Account");
            }

            var profileResult = await _apiClient.GetProfileAsync(user.UserId, user.Token);
            if (!profileResult.Success || profileResult.Data == null)
            {
                TempData["ErrorMessage"] = "Unable to load your profile right now.";
                return RedirectToAction("Index", "Dashboard");
            }

            if (!profileResult.Data.IsProfileComplete)
            {
                TempData["ErrorMessage"] = "Please complete your profile before applying for a loan.";
                return RedirectToAction("Edit", "Profile");
            }

            return View(new LoanApplicationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(LoanApplicationViewModel model)
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var profileResult = await _apiClient.GetProfileAsync(user.UserId, user.Token);
            if (!profileResult.Success || profileResult.Data == null || !profileResult.Data.IsProfileComplete)
            {
                TempData["ErrorMessage"] = "Please complete your profile before applying for a loan.";
                return RedirectToAction("Edit", "Profile");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _apiClient.ApplyLoanAsync(user, model, user.Token);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["StatusMessage"] = "Loan application submitted successfully. It is now pending review.";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _apiClient.GetLoanByIdAsync(id, user.Token);
            if (!result.Success || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "Dashboard");
            }

            var loan = result.Data;
            if (user.Role == "Customer" && loan.UserId != user.UserId)
            {
                TempData["ErrorMessage"] = "You do not have access to that loan.";
                return RedirectToAction("Index", "Dashboard");
            }

            var viewModel = new LoanDetailsViewModel
            {
                CurrentUser = user,
                Loan = loan,
                CanReview = user.Role is "Admin" or "Manager"
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDecision(int id, string status, string? reason)
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (user.Role is not ("Admin" or "Manager"))
            {
                TempData["ErrorMessage"] = "Only admins and managers can review loans.";
                return RedirectToAction("Details", new { id });
            }

            if (status == "Rejected" && string.IsNullOrWhiteSpace(reason))
            {
                TempData["ErrorMessage"] = "Please add a rejection reason before rejecting the loan.";
                return RedirectToAction("Details", new { id });
            }

            var result = await _apiClient.UpdateLoanDecisionAsync(id, user.Role, status, reason, user.Token);
            TempData[result.Success ? "StatusMessage" : "ErrorMessage"] = result.Success
                ? $"{user.Role} review submitted as {status}."
                : result.Message;

            return RedirectToAction("Details", new { id });
        }
    }
}
