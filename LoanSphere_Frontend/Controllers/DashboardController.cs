using LoanSphere_Frontend.Models;
using LoanSphere_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CurrentUserService _currentUserService;
        private readonly LoanSphereApiClient _apiClient;

        public DashboardController(CurrentUserService currentUserService, LoanSphereApiClient apiClient)
        {
            _currentUserService = currentUserService;
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var user = _currentUserService.GetCurrentUser();
            if (user == null)
            {
                TempData["ErrorMessage"] = "Please log in to continue.";
                return RedirectToAction("Login", "Account");
            }

            var profileResult = await _apiClient.GetProfileAsync(user.UserId, user.Token);
            var myLoansResult = await _apiClient.GetLoansByUserAsync(user.UserId, user.Token);
            var loadErrors = new List<string>();

            if (!profileResult.Success)
            {
                loadErrors.Add(profileResult.Message);
            }

            if (!myLoansResult.Success)
            {
                loadErrors.Add(myLoansResult.Message);
            }

            var viewModel = new DashboardViewModel
            {
                CurrentUser = user,
                Profile = profileResult.Data,
                MyLoans = myLoansResult.Data ?? new List<LoanSummaryViewModel>(),
                RequiresProfileCompletion = !(profileResult.Data?.IsProfileComplete ?? false)
            };

            if (viewModel.CanManageLoans)
            {
                var allLoansResult = await _apiClient.GetAllLoansAsync(user.Token);
                if (!allLoansResult.Success)
                {
                    loadErrors.Add(allLoansResult.Message);
                }

                viewModel.ManagedLoans = allLoansResult.Data ?? new List<LoanSummaryViewModel>();
            }

            if (loadErrors.Count > 0)
            {
                TempData["ErrorMessage"] = string.Join(" ", loadErrors.Distinct());
            }

            return View(viewModel);
        }
    }
}
