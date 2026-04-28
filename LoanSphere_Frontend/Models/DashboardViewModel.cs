namespace LoanSphere_Frontend.Models
{
    public class DashboardViewModel
    {
        public AuthenticatedUser CurrentUser { get; set; } = new();
        public ProfileViewModel? Profile { get; set; }
        public List<LoanSummaryViewModel> MyLoans { get; set; } = new();
        public List<LoanSummaryViewModel> ManagedLoans { get; set; } = new();
        public bool RequiresProfileCompletion { get; set; }
        public bool CanManageLoans => CurrentUser.Role is "Admin" or "Manager";
    }
}
