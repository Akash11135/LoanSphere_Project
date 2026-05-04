using LoanManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoanManagement.Models;
using LoanManagement.DTOs;

namespace LoanManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("admin")]

    public class AdminController : Controller
    {
        private readonly AdminService _service;
        public AdminController(AdminService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var resp = await _service.GetAdminDashboardService();
            if(resp == null)
            {
                return Ok(new ApiResponse<object>(404, "unable to find data for dashboard.", resp));
            }
            return Ok(new ApiResponse<object>(200, "data successfully found for admin dashboard.", resp));
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("getbyid/{loanId}")]
        public async Task<IActionResult> GetLoanDetailsById(int loanId)
        {
            var resp = await _service.GetByIdService(loanId);
            if (resp == null)
            {
                return Ok(new ApiResponse<object>(404, $"unable to find data for loanId:{loanId}", resp));
            }
            return Ok(new ApiResponse<object>(200, $"data successfully found for loanId:{loanId}", resp));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("review/{loanId}")]
        public async Task<IActionResult> ReviewLoan(int loanId, ReviewDto reviewLoan)
        {
            var resp = await _service.ReviewLoanService(loanId, reviewLoan);
            if(loanId != reviewLoan.LoanId)
            {
                return Ok(new ApiResponse<object>(404, $"Id missmatch for loanId:{loanId}", resp));
            }
            if (resp == null)
            {
                return Ok(new ApiResponse<object>(404, $"unable to find data for loanId:{loanId}", resp));
            }
            return Ok(new ApiResponse<object>(200, $"data successfully reviewed for loanId:{loanId}", resp));
        }
       
    }
}