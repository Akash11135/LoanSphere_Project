using LoanManagement.Data;
using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Controllers
{
    [ApiController]
    [Route("emi")]
    public class EMIController : Controller
    {
        private readonly AppDbContext _context;
        public EMIController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("payment/{emiId}")]
        public async Task<IActionResult> EmiPayment(int emiId, EMIDto emi)
        {
            if (emi == null)
                return BadRequest("EMI payload is required.");

            var existing = await _context.EMISchedules
                .Include(e => e.Loan)
                .FirstOrDefaultAsync(e => e.Id == emiId);

            if (existing == null)
            {
                return Ok(new ApiResponse<Object>(404, $"No Emi found with emiId:{emiId}", existing));
            }

            existing.IsPaid = emi.IsPaid;
            existing.PaidAt = emi.PaidAt;

            var loan = existing.Loan;

            if (loan == null)
                return StatusCode(500, "Loan relationship not loaded.");

            loan.TotalPaid += existing.EMIAmount;

            var allPaid = await _context.EMISchedules
                .Where(l => l.LoanId == loan.LoanId && l.Id != emiId)
                .AllAsync(x => x.IsPaid);

            if (allPaid)
            {
                loan.Status = "completed"; // typo fixed
            }

            loan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Object>(200, $"Successfully Updated the EMISchedule for Id:{emiId}.", existing));
        }

        [HttpGet("emiById/{emiId}")]
        public async Task<IActionResult> EmiById(int emiId) 
        {
            var emi = await _context.EMISchedules.FirstOrDefaultAsync(e=>e.Id==emiId);
            if (emi == null)
            {
                return Ok(new ApiResponse<Object>(404, $"Unable to find emi by Id:{emiId}", emi));

            }
            return Ok(new ApiResponse<Object>(404, $"Successfully found emi by Id:{emiId}", emi));

        }
    }
}
