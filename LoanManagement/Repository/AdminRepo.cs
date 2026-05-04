using LoanManagement.Data;
using LoanManagement.DTOs;
using LoanManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Repository
{
    public class AdminRepo 
    {
        private readonly AppDbContext _context;

        public AdminRepo(AppDbContext context)
        {
            _context = context;
        }


     
        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var totalAmount = await _context.Loans
                .Where(l => l.IsRejected==false && l.IsVerifiedByAdmin==true)
                .SumAsync(l => l.Amount);

            var ActiveLoans = await _context.Loans
                .Include(l => l.EMISchedules)
                .Where(l => l.DocsVerified == "true" && l.IsRejected==false&& l.IsVerifiedByAdmin==true)
                .ToListAsync();

            var ReviewLoans = await _context.Loans
                .Include(l => l.EMISchedules)
                .Where(l => l.DocsVerified == "false" && l.IsRejected==false && l.IsVerifiedByAdmin==false)
                .ToListAsync();

            var RejectedLoans = await _context.Loans
                .Include(l => l.EMISchedules)
                .Where(l => l.IsRejected == true).ToListAsync();

            var dashboard = new AdminDashboardDto
            {
                TotalAmount = totalAmount,
                ReviewLoans =  ReviewLoans,
                ActiveLoans = ActiveLoans,
                RejectedLoans = RejectedLoans,
            };

            return dashboard;
        }


  
        public async Task<Loan> GetLoanById(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.EMISchedules)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

        return loan;
        }
    
        public async Task<Loan> ReviewLoanAsync(int loanId , ReviewDto reviewLoan)
        {
            var existing = await _context.Loans.FindAsync(loanId);
            existing.UpdatedAt = DateTime.UtcNow;
            existing.DocsVerified = reviewLoan.DocsVerified;
            existing.IsRejected = reviewLoan.IsRejected;
            existing.IsVerifiedByAdmin = reviewLoan.IsVerifiedByAdmin;

            await _context.SaveChangesAsync();
            return existing;

        }
    }
}