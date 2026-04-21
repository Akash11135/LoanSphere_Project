using LoanAuth.DTOs;

namespace LoanAuth.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string? UserId)> RegisterCustomerAsync(RegisterCustomerDto dto);

        Task<(bool Success, string Message, string? UserId)> RegisterAdminAsync(RegisterAdminDto dto);

        Task<(bool Success, string Message, string? UserId)> RegisterManagerAsync(RegisterManagerDto dto);

        Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto dto);
    }
}