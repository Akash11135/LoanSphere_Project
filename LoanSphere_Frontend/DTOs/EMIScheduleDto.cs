namespace LoanSphere_Frontend.DTOs
{
    public class EmiScheduleDto
    {
        public int Id { get; set; }

        public int MonthNumber { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }

        public decimal EmiAmount { get; set; }
        public decimal PrincipalComponent { get; set; }
        public decimal InterestComponent { get; set; }

        public decimal RemainingBalance { get; set; }

        public bool IsPaid { get; set; }
    }
}
