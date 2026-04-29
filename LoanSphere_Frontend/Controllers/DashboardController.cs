using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _http;

        public DashboardController(HttpClient http)
        {
            _http = http;
        }

        public async Task<IActionResult> Index()
        {
            var resp = await _http.GetFromJsonAsync<ApiResponse<DashboardDto>>(
                "http://localhost:5002/loan/dashboard/2");

            if (resp == null || resp.Status != 200)
            {
                return NotFound();
            }

            return View(resp.Data);
        }

        public async Task<IActionResult> LoanDetails(int userId, int loanId)
        {
            var resp = await _http.GetFromJsonAsync<ApiResponse<Loan>>(
                $"http://localhost:5002/loan/getloanbyid/{userId}/{loanId}");

            if (resp == null || resp.Status != 200)
            {
                return NotFound();
            }

            return View(resp.Data);
        }

        [HttpGet]
        public IActionResult ApplyNew()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyNew(int userId, CreateLoanDto loan)
        {
            var newLoan = new CreateLoanDto()
            {
                UserId = userId,
                Amount = loan.Amount,
                Purpose = loan.Purpose,
                LoanType = loan.LoanType,
                TermInMonths = loan.TermInMonths,
            };

            var resp = await _http.PostAsJsonAsync(
                "http://localhost:5002/loan/create", newLoan);

            if (resp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            TempData["Error"] = "Unable to create loan";
            return RedirectToAction("ApplyNew");
        }
    }
}