using LoanManagement.DTOs;
using LoanManagement.Models;
using LoanManagement.Repository;

namespace LoanManagement.Services
{
    public class AdminService
    {
        private readonly AdminRepo _repo; 

        public AdminService(AdminRepo repo)
        {
            _repo = repo; 
        }
        public async Task<Loan> GetByIdService(int loanId)
        {
            return await _repo.GetLoanById(loanId);
        }

        public async Task<AdminDashboardDto> GetAdminDashboardService()
        {
            return await _repo.GetDashboardAsync();
        }
        public async Task<Loan> ReviewLoanService(int loanId , ReviewDto reviewLoan)
        {
            return await _repo.ReviewLoanAsync(loanId, reviewLoan);
        }
    }
}
