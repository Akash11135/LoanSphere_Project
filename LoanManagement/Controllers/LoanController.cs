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


        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _loanService.GetAllService();
            if (resp == null)
            {
                return Ok(new { mess = $"Unable to find all Loans BEC" });
            }
            return Ok(resp);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resp = await _loanService.GetByIdService(id);
            if (resp == null)
            {
                return Ok(new { mess = $"Loan with id:{id} not found BEC." });
            }
            return Ok(resp);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, Loan loan)
        {
            if (id != loan.LoanId)
            {
                return BadRequest("Id not found BEC");
            }
            var resp = await _loanService.UpdateService(id, loan);
            if (resp == null)
            {
                return Ok(new { mess = $"Loan with id:{id} not found BEC." });
            }
            return Ok(resp);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLoanDto loan)
        {
            var resp = await _loanService.CreateService(loan);

            if (resp == null)
            {
                return Ok(new { mess = $"Loan not created BEC." });
            }
            return Ok(resp);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _loanService.DeleteService(id);
            if (res == null)
            {
                return Ok(new { mess = "Unable to delete" });
            }
            return Ok(res);
        }
    }
}
