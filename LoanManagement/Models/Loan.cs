namespace LoanManagement.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public double Amount { get; set; }
        public double InterestRate { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public int TermInMonths { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string DocsVerified { get; set; } = string.Empty;
        public string AdminReviewStatus { get; set; } = string.Empty;
        public string? AdminReviewReason { get; set; }
        public DateTime? AdminReviewedAt { get; set; }
        public string ManagerReviewStatus { get; set; } = string.Empty;
        public string? ManagerReviewReason { get; set; }
        public DateTime? ManagerReviewedAt { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<EMISchedule> EMISchedules { get; set; } = new();

        public double EMIAmount { get; set; }
        public double TotalPayable { get; set; }
        public double TotalPaid { get; set; }
    }
}
