namespace LoanSphere_Frontend.Models
{
    public class LoanDetailsViewModel
    {
        public AuthenticatedUser CurrentUser { get; set; } = new();
        public LoanSummaryViewModel Loan { get; set; } = new();
        public bool CanReview { get; set; }
        public bool CanSeePaymentSchedule => Loan.IsFullyApproved;
        public string ReviewerRole => CurrentUser.Role;
    }
}
