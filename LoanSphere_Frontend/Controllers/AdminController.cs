using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace LoanSphere_Frontend.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _http;

        public AdminController(HttpClient http)
        {
            _http = http;
        }

       
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Unauthorised", "Error");
            }

            token = token.Trim('"');

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "http://localhost:5002/admin/dashboard"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Unable to fetch dashboard";
                return RedirectToAction("Nodata", "Error");
            }

            var resp = await response.Content.ReadFromJsonAsync<ApiResponse<AdminDashboardDto>>();

            if (resp == null || resp.Status != 200)
            {
                return RedirectToAction("Nodata", "Error");
            }

            return View(resp.Data);
        }

        public async Task<IActionResult> LoanDetails(string userId, int loanId)
        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Unauthorised", "Error");
            }

            token = token.Trim('"');

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5002/admin/getbyid/{loanId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load loan details";
                return RedirectToAction("Index");
            }

            var resp = await response.Content.ReadFromJsonAsync<ApiResponse<Loan>>();

            return View(resp.Data);
        }

      
        [HttpGet]
        [Route("Admin/ReviewLoan/{loanId}")]
        public IActionResult ReviewLoan(int loanId)
        {
            // ✅ PASS LoanId to view
            var model = new ReviewDto
            {
                LoanId = loanId
            };

            return View(model);
        }

  
        [HttpPost]
        public async Task<IActionResult> ReviewLoan(ReviewDto reviewLoan)
        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Unauthorised", "Error");
            }

            token = token.Trim('"');

          
            reviewLoan.UpdatedAt = DateTime.UtcNow;

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"http://localhost:5002/admin/review/{reviewLoan.LoanId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(reviewLoan);

            var response = await _http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content); // 🔥 Debug log

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "API Error: " + content;
                return RedirectToAction("Index", "Admin");
            }

            TempData["Success"] = "Loan reviewed successfully";

            return RedirectToAction("Index", "Admin");
        }
    }
}