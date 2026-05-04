using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:5002/emi/emiById/{emiId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Trim('"'));

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "EMI not found";
                return RedirectToAction("Index", "Dashboard");
            }

            var resp = await response.Content.ReadFromJsonAsync<ApiResponse<EMISchedule>>();

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
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var content = new EMIDto()
            {
                IsPaid = emi.IsPaid,
                PaidAt = DateTime.UtcNow
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"http://localhost:5002/emi/payment/{emiId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Trim('"'));

            request.Content = JsonContent.Create(content);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadFromJsonAsync<ApiResponse<EMISchedule>>();

                TempData["Success"] = "Payment successful!";

                return RedirectToAction("LoanDetails", "Dashboard", new
                {
                    userId = 2, // you can later extract from JWT
                    loanId = resp?.Data?.LoanId
                });
            }

            TempData["Error"] = "Payment failed!";
            return RedirectToAction("Payments", new { emiId });
        }
    }
}