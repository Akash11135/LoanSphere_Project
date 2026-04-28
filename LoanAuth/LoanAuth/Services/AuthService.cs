using LoanAuth.DTOs;
using LoanAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanAuth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // 🔹 CUSTOMER REGISTER
        public async Task<(bool Success, string Message, string? UserId)> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return (false, "Passwords do not match.", null);

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.Phone
            };

            return await RegisterUserAsync(user, dto.Password, "Customer");
        }

        // 🔹 ADMIN REGISTER
        public async Task<(bool Success, string Message, string? UserId)> RegisterAdminAsync(RegisterAdminDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return (false, "Passwords do not match.", null);

            if (dto.AdminSecretKey != _configuration["SecretKeys:AdminSecretKey"])
                return (false, "Invalid admin secret key.", null);

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.Phone
            };

            return await RegisterUserAsync(user, dto.Password, "Admin");
        }

        // 🔹 MANAGER REGISTER
        public async Task<(bool Success, string Message, string? UserId)> RegisterManagerAsync(RegisterManagerDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return (false, "Passwords do not match.", null);

            if (dto.ManagerSecretKey != _configuration["SecretKeys:ManagerSecretKey"])
                return (false, "Invalid manager secret key.", null);

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.Phone
            };

            return await RegisterUserAsync(user, dto.Password, "Manager");
        }

        // 🔹 LOGIN
        public async Task<(bool Success, string Message, string? Token, string? UserId, string? Role, string? FullName, string? Email)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return (false, "Invalid email or password.", null, null, null, null, null);

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            var primaryRole = roles.FirstOrDefault();

            return (true, "Login successful.", token, user.Id, primaryRole, user.FullName, user.Email);
        }

        // 🔹 COMMON REGISTER METHOD
        private async Task<(bool Success, string Message, string? UserId)> RegisterUserAsync(
            ApplicationUser user, string password, string role)
        {
            if (await _userManager.FindByEmailAsync(user.Email!) != null)
                return (false, "Email is already registered.", null);

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            await _userManager.AddToRoleAsync(user, role);

            return (true, $"{role} registered successfully.", user.Id);
        }

        // 🔹 JWT TOKEN
        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
