using Azure.Core;
using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Unauthorised", "Error");
            }

            token = token.Trim('"'); // safety fix

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "http://localhost:5002/loan/dashboard"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "unable to fetch.";
                return RedirectToAction("Nodata", "Error");
            }

          
            var resp = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardDto>>();

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

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5002/loan/getloanbyid/{loanId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response == null || !response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load loan details";
                return RedirectToAction("Index");
            }

            var resp = await response.Content.ReadFromJsonAsync<ApiResponse<Loan>>();

            TempData["Success"] = "Loan details loaded";

            return View(resp.Data);
        }

        [HttpGet]
        public IActionResult ApplyNew()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyNew(CreateLoanDto loan)
        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Unauthorised", "Error");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

            var newLoan = new CreateLoanDto()
            {
                UserId = userId,
                Amount = loan.Amount,
                Purpose = loan.Purpose,
                LoanType = loan.LoanType,
                TermInMonths = loan.TermInMonths,
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5002/loan/create");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(newLoan);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Something went wrong while creating loan";
                return RedirectToAction("ApplyNew");
            }

            var resp = await response.Content.ReadFromJsonAsync<ApiResponse<CreateLoanDto>>();

            if (resp.Status == 500)
            {
                TempData["Error"] = "Unable to create loan";
                return RedirectToAction("ApplyNew");
            }

            // ✅ SUCCESS TOAST
            TempData["Success"] = "Loan created successfully";

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllLoans()
        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Session expired";
                return RedirectToAction("Unauthorised", "Error");
            }

            token = token.Trim('"');

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5002/loan/getall");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to fetch loans";
                return RedirectToAction("Index");
            }

            var res = await response.Content.ReadFromJsonAsync<ApiResponse<List<Loan>>>();

            if (res.Status == 500)
            {
                TempData["Error"] = "No loans found";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "All loans loaded successfully";

            return View(res.Data);
        }
    }
}

