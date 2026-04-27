using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

            var resp = await _http.GetFromJsonAsync<ApiResponse<DashboardDto>>("http://localhost:5002/loan/dashboard/1");
            if (resp.Status == 404)
            {
                return NotFound();
            }
           
            return View(resp.Data);
        }

        public async Task<IActionResult> LoanDetails()
        {
            var resp = await _http.GetFromJsonAsync<ApiResponse<Loan>>("http://localhost:5002/loan/getloanbyid/1/14");
            if (resp.Status != 200)
            {
                return NotFound();
            }
            return View(resp.Data);
        }

        public async Task<IActionResult> ApplyNew()
        {
            return View();
        }
    }
}
