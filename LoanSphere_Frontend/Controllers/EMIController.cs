using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanSphere_Frontend.Controllers
{
    public class EMIController : Controller
    {
        private readonly HttpClient _http;

        public EMIController(HttpClient http)
        {
            _http = http;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Payments(int emiId)
        {
            var resp = await _http.GetFromJsonAsync<ApiResponse<EMISchedule>>(
                $"http://localhost:5002/emi/emiById/{emiId}");

            if (resp != null)
            {
                ViewData["Title"] = "EMI Payment";
                return View(resp.Data);
            }

            TempData["Error"] = "EMI not found";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Payments(int emiId, EMIDto emi)
        {
            var content = new EMIDto()
            {
                IsPaid = emi.IsPaid,
                PaidAt = DateTime.UtcNow
            };

            var response = await _http.PostAsJsonAsync(
                $"http://localhost:5002/emi/payment/{emiId}", content);

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadFromJsonAsync<ApiResponse<EMISchedule>>();

                TempData["Success"] = "Payment successful!";

                return RedirectToAction("LoanDetails", "Dashboard", new
                {
                    userId = 2, // replace with real userId later
                    loanId = resp?.Data?.LoanId
                });
            }

            TempData["Error"] = "Payment failed!";
            return RedirectToAction("Payments", new { emiId });
        }
    }
}