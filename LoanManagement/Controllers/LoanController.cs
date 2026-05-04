using LoanManagement.DTOs;
using LoanManagement.Models;
using LoanManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanManagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("loan")]
    public class LoanController : Controller
    {
        private readonly LoanService _loanService;

        public LoanController(LoanService loanService)
        {
            _loanService = loanService;
        }

        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

       
        [Authorize(Roles = "Customer")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            var respData = await _loanService.GetAllService(userId);

            if (respData == null)
            {
                return Ok(new ApiResponse<object>(404, "No Loan data found.", respData));
            }

            return Ok(new ApiResponse<object>(200, "Loan Data found successfully.", respData));
        }

    
        [Authorize(Roles = "Customer")]
        [HttpGet("getloanbyid/{loanId}")]
        public async Task<IActionResult> GetById(int loanId)
        {
            var userId = GetUserId();

            var resp = await _loanService.GetByIdService(userId, loanId);

            return Ok(new ApiResponse<object>(200, resp.Mess, resp.loan));
        }

     
        [Authorize(Roles = "Customer")]
        [HttpPut("update/{loanId}")]
        public async Task<IActionResult> Update(int loanId, UpdateLoanDto loan)
        {
            var userId = GetUserId();

            var resp = await _loanService.UpdateService(userId, loanId, loan);

            return Ok(new ApiResponse<object>(200, resp.Mess, resp.loan));
        }

      
        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLoanDto loan)
        {
            var resp = await _loanService.CreateService(loan);

            if (resp == null)
            {
                return Ok(new ApiResponse<object>(500, "Loan not created backend.", resp));
            }

            return Ok(new ApiResponse<object>(200, "Created Loan successfully", resp));
        }

       
        [Authorize(Roles = "Customer")]
        [HttpDelete("delete/{loanId}")]
        public async Task<IActionResult> Delete(int loanId)
        {
            var userId = GetUserId();

            var resp = await _loanService.DeleteService(userId, loanId);

            if (resp == null)
            {
                return Ok(new ApiResponse<object>(500, "Unable to Delete", resp));
            }

            return Ok(new ApiResponse<object>(200, "Deleted Successfully", resp));
        }

       
        [Authorize(Roles = "Customer")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> DashboardContent()
        {
            var userId = GetUserId();

            var res = await _loanService.DashBoardService(userId);

            if (res == null)
            {
                return Ok(new ApiResponse<object>(500, "Unable to get dashboard backend.", res));
            }

            return Ok(new ApiResponse<object>(200, "Dashboard details fetched successfully.", res));
        }
    }
}