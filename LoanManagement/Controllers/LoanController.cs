using LoanManagement.DTOs;
using LoanManagement.Models;
using LoanManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Controllers
{
    [ApiController]
    [Route("loan")]
    public class LoanController : Controller
    {
        private readonly LoanService _loanService;

        public LoanController(LoanService loanService)
        {
            _loanService = loanService;
        }


        [HttpGet("getall/{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var respData = await _loanService.GetAllService(userId);
            
            if (respData == null)
            {
                return Ok(new ApiResponse<Object>(404, $"No Loan data found for {userId} backend.",respData));
            }
            return Ok(new ApiResponse<Object>(200, "Loan Data found successfully.", respData));
        }


        [HttpGet("getloanbyid/{userId}/{loanId}")]
        public async Task<IActionResult> GetById(int userId , int loanId)
        {
            var resp = await _loanService.GetByIdService(userId , loanId);
         
            return Ok(new ApiResponse<Object>(200, resp.Mess ,resp.loan));
        }


        [HttpPut("update/{userId}/{loanId}")]
        public async Task<IActionResult> Update(int userId, int loanId, UpdateLoanDto loan)
        {
  
            var resp = await _loanService.UpdateService(userId, loanId, loan);
                return Ok(new ApiResponse<Object>(200, resp.Mess, resp.loan));
         
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLoanDto loan)
        {
            var resp = await _loanService.CreateService(loan);

            if (resp == null)
            {
                return Ok(new ApiResponse<Object> (500, $"Loan not created backend." ,resp ));
            }
            return Ok(new ApiResponse<Object>(200, "Created Loan successfully", resp));
        }

        [HttpDelete("delete/{userId}/{loanId}")]
        public async Task<IActionResult> Delete(int userId,int loanId)
        {
            var resp = await _loanService.DeleteService(userId , loanId);
            if (resp == null)
            {
                return Ok(new ApiResponse<Object>(500, "Unable to Delete", resp));
            }
            return Ok(new ApiResponse<Object>(200, "Deleted Successfully", resp));
        }

        [HttpGet("dashboard/{userId}")]
        public async Task<IActionResult> DashboardContent(int userId)
        {
            var res = await _loanService.DashBoardService(userId);
            if (res == null)
            {
                return Ok(new ApiResponse<Object>(500, "Unable to get dashboard backedn.", res));
            }
            return Ok(new ApiResponse<Object>(200, "Dashboard details fetched successfully.",res));
        }
    }
}
