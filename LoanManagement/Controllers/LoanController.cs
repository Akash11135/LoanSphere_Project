using LoanManagement.DTOs;
using LoanManagement.Services;
using LoanManagement.Models;
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


        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _loanService.GetAllService();
            return Ok(resp);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var resp = await _loanService.GetByUserService(userId);
            return Ok(resp);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id) 
        { 
            var resp = await _loanService.GetByIdService(id);
            return Ok(resp);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id , Loan loan)
        {
            if (id != loan.LoanId)
            {
                return BadRequest("Id not found");
            }
            var resp = await _loanService.UpdateService(id,loan);
            return Ok(resp);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLoanDto loan)
        {
            var resp = await _loanService.CreateService(loan);
            return Ok(resp);
        }

        [HttpPatch("{id}/decision")]
        public async Task<IActionResult> UpdateDecision(int id, [FromBody] UpdateLoanDecisionDto dto)
        {
            var resp = await _loanService.UpdateDecisionService(id, dto.ReviewerRole, dto.Status, dto.Reason);
            return Ok(resp);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id) 
        { 
            var res = await _loanService.DeleteService(id);
            return Ok(res);
        }
    }
}
