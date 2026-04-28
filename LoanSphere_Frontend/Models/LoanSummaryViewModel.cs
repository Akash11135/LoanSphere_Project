namespace LoanSphere_Frontend.Models
{
    public class LoanSummaryViewModel
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
        public List<EmiScheduleViewModel> EmiSchedules { get; set; } = new();
        public double EmiAmount { get; set; }
        public double TotalPayable { get; set; }
        public double TotalPaid { get; set; }
        public double OutstandingAmount => Math.Max(0, TotalPayable - TotalPaid);
        public bool IsFullyApproved =>
            Status.Equals("Approved", StringComparison.OrdinalIgnoreCase);
        public bool IsRejected =>
            Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase);

        public EmiScheduleViewModel? NextPendingInstallment =>
            EmiSchedules
                .Where(item => !item.IsPaid)
                .OrderBy(item => item.MonthNumber)
                .FirstOrDefault();

        public List<EmiScheduleViewModel> PaidInstallments =>
            EmiSchedules
                .Where(item => item.IsPaid)
                .OrderByDescending(item => item.PaidAt ?? item.DueDate)
                .ToList();

        public string ReviewSummary
        {
            get
            {
                if (IsFullyApproved)
                {
                    return "Approved by admin and manager";
                }

                if (IsRejected)
                {
                    return GetRejectionSummary();
                }

                return $"{NormalizeDecision(AdminReviewStatus)} / {NormalizeDecision(ManagerReviewStatus)} reviews";
            }
        }

        public string? PrimaryReviewReason
        {
            get
            {
                if (IsRejected)
                {
                    if (AdminReviewStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(AdminReviewReason))
                    {
                        return AdminReviewReason;
                    }

                    if (ManagerReviewStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(ManagerReviewReason))
                    {
                        return ManagerReviewReason;
                    }
                }

                return null;
            }
        }

        private string GetRejectionSummary()
        {
            var rejectedBy = new List<string>();

            if (AdminReviewStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                rejectedBy.Add("admin");
            }

            if (ManagerReviewStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                rejectedBy.Add("manager");
            }

            return rejectedBy.Count == 0
                ? "Rejected"
                : $"Rejected by {string.Join(" and ", rejectedBy)}";
        }

        private static string NormalizeDecision(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "Pending" : value;
        }
    }
}
