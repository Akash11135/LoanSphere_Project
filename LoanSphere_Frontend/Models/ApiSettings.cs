namespace LoanSphere_Frontend.Models
{
    public class ApiSettings
    {
        public const string SectionName = "ApiSettings";

        public string AuthBaseUrl { get; set; } = string.Empty;
        public string ProfileBaseUrl { get; set; } = string.Empty;
        public string LoanBaseUrl { get; set; } = string.Empty;
    }
}
