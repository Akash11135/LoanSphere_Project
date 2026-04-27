using LoanManagement.Models;

namespace LoanManagement.DTOs
{
    public class DashboardDto
    {
        public double TotalAmount { get; set; }
        public List<Loan> PendingLoans { get; set; } 
        public List<Loan> VerifiedLoans { get; set; } 
        
        public List<EMISchedule> DueLoans { get; set; }

    }
}
