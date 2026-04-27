namespace LoanSphere_Frontend.DTOs
{
    public class LoanDetailDto
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }

        public string LoanType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public decimal EmiAmount { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal TotalPaid { get; set; }

        public double InterestRate { get; set; }
        public int TermInMonths { get; set; }

        public string DocsVerified { get; set; } = "false";

        public DateTime AppliedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<EmiScheduleDto> EmiSchedules { get; set; } = new();
    }
}
