using System.Text.Json.Serialization;

namespace LoanManagement.Models
{
    public class EMISchedule
    {
        public int Id { get; set; }

        public int LoanId { get; set; }

        [JsonIgnore]
        public Loan? Loan { get; set; }

        public int MonthNumber { get; set; }

        public DateTime DueDate { get; set; }

        public double EMIAmount { get; set; }

        public double PrincipalComponent { get; set; }

        public double InterestComponent { get; set; }

        public double RemainingBalance { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime? PaidAt { get; set; }
    }
}
