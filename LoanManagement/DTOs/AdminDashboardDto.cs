using LoanManagement.Models;

namespace LoanManagement.DTOs
{
    public class AdminDashboardDto
    {
        public double TotalAmount { get; set; }

        //loans to be rewiewed.
        public List<Loan> ReviewLoans { get; set; }

        //active loans  where docevrifies=true
        public List<Loan> ActiveLoans { get; set; }

        public List<Loan> RejectedLoans { get; set; }
    
    }
}
