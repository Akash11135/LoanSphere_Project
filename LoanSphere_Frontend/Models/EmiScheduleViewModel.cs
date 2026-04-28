namespace LoanSphere_Frontend.Models
{
    public class EmiScheduleViewModel
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public int MonthNumber { get; set; }
        public DateTime DueDate { get; set; }
        public double EmiAmount { get; set; }
        public double PrincipalComponent { get; set; }
        public double InterestComponent { get; set; }
        public double RemainingBalance { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
