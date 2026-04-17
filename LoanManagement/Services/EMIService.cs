using LoanManagement.Data;
using LoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Services
{
    public class EMIService
    {
        private readonly AppDbContext _context;


        public EMIService(AppDbContext context) {
            _context = context;
        }

        public async Task<(double emi, double totalPayable)> CalculateEmi(
            int totalMonths,
            double amount,
            double interestRate,
            DateTime startDate,
            Loan loan)
        {
            double monthlyRate = interestRate / 12 / 100;

        
            double emi = amount * monthlyRate * Math.Pow(1 + monthlyRate, totalMonths)
                       / (Math.Pow(1 + monthlyRate, totalMonths) - 1);

            double totalPayable = emi * totalMonths;

            emi = Math.Round(emi, 2);

            var schedules = new List<EMISchedule>();

            double balance = amount;
            DateTime dueDate = startDate;

            for (int i = 1; i <= totalMonths; i++)
            {
                double interest = balance * monthlyRate;
                double principal = emi - interest;

                balance -= principal;
                dueDate = dueDate.AddMonths(1);

                schedules.Add(new EMISchedule
                {
                    LoanId = loan.LoanId,
                    MonthNumber = i,
                    DueDate = dueDate,
                    EMIAmount = emi,
                    InterestComponent = Math.Round(interest, 2),
                    PrincipalComponent = Math.Round(principal, 2),
                    RemainingBalance = Math.Round(balance, 2),
                    IsPaid = false,
                    PaidAt = null
                });
            }

            
            await _context.EMISchedules.AddRangeAsync(schedules);
            await _context.SaveChangesAsync();

            return (Math.Round(emi, 2), Math.Round(totalPayable, 2));
        }

        public async Task<List<EMISchedule>> GetEMISchedule(int id)
        {
            var resp =  await _context.EMISchedules
                         .Where(e => e.LoanId == id)
                         .OrderBy(e => e.MonthNumber)
                         .ToListAsync();

            await _context.SaveChangesAsync();
            return resp;
        }
    }
}
