using LoanProfile.DTOs;

namespace LoanProfile.Services
{
    public interface IProfileService
    {
        Task CreateProfileAsync(CreateProfileDto dto);
        Task UpdateProfileAsync(string userId, UpdateProfileDto dto);
        Task<ProfileResponseDto?> GetProfileAsync(string userId);
    }
}