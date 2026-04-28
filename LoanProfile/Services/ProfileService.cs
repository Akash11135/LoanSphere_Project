using LoanProfile.Data;
using LoanProfile.DTOs;
using LoanProfile.Helpers;
using LoanProfile.Models;
using LoanProfile.Services;
using Microsoft.EntityFrameworkCore;

namespace LoanProfile.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ProfileDbContext _context;

        public ProfileService(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task CreateProfileAsync(CreateProfileDto dto)
        {
            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == dto.UserId);

            if (existingProfile != null)
            {
                existingProfile.FullName = dto.FullName;
                existingProfile.Email = dto.Email;
                existingProfile.Phone = dto.Phone;
                existingProfile.Role = dto.Role;

                await _context.SaveChangesAsync();
                return;
            }

            var profile = new UserProfile
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                CibilScore = CibilScoreGenerator.GenerateInitialScore()
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return;

            profile.ProfilePictureUrl = dto.ProfilePictureUrl;
            profile.PanCardNumber = dto.PanCardNumber;
            profile.AadhaarNumber = dto.AadhaarNumber;

            await _context.SaveChangesAsync();
        }

        public async Task<ProfileResponseDto?> GetProfileAsync(string userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return null;

            return new ProfileResponseDto
            {
                FullName = profile.FullName,
                Email = profile.Email,
                Phone = profile.Phone,
                Role = profile.Role,
                ProfilePictureUrl = profile.ProfilePictureUrl,
                PanCardNumber = profile.PanCardNumber,
                AadhaarNumber = profile.AadhaarNumber,
                CibilScore = profile.CibilScore,
                IsProfileComplete = !string.IsNullOrWhiteSpace(profile.ProfilePictureUrl)
                    && !string.IsNullOrWhiteSpace(profile.PanCardNumber)
                    && !string.IsNullOrWhiteSpace(profile.AadhaarNumber)
            };
        }
    }
}
