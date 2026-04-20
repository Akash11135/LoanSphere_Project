using LoanAuth.DTOs;

namespace LoanAuth.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string? Token)> RegisterCustomerAsync(RegisterCustomerDto dto);
        Task<(bool Success, string Message, string? Token)> RegisterAdminAsync(RegisterAdminDto dto);
        Task<(bool Success, string Message, string? Token)> RegisterManagerAsync(RegisterManagerDto dto);
        Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto dto);
    }
}
