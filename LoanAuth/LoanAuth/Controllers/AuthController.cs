using LoanAuth.DTOs;
using LoanAuth.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace LoanAuth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;

        public AuthController(IAuthService authService, IHttpClientFactory httpClientFactory)
        {
            _authService = authService;
            _httpClient = httpClientFactory.CreateClient();
        }

        // 🔹 CUSTOMER REGISTER
        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDto dto)
        {
            var (success, message, _) = await _authService.RegisterCustomerAsync(dto);

            if (!success)
                return BadRequest(new { message });

            // 🔥 CALL PROFILE SERVICE
            await CreateProfile(dto.Email, dto.FullName, dto.Phone, "Customer");

            return Ok(new { message });
        }

        // 🔹 ADMIN REGISTER
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto dto)
        {
            var (success, message, _) = await _authService.RegisterAdminAsync(dto);

            if (!success)
                return BadRequest(new { message });

            await CreateProfile(dto.Email, dto.FullName, dto.Phone, "Admin");

            return Ok(new { message });
        }

        // 🔹 MANAGER REGISTER
        [HttpPost("register/manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterManagerDto dto)
        {
            var (success, message, _) = await _authService.RegisterManagerAsync(dto);

            if (!success)
                return BadRequest(new { message });

            await CreateProfile(dto.Email, dto.FullName, dto.Phone, "Manager");

            return Ok(new { message });
        }

        // 🔹 LOGIN (UNCHANGED)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (success, message, token) = await _authService.LoginAsync(dto);
            return success
                ? Ok(new { message, token })
                : Unauthorized(new { message });
        }

        // 🔥 COMMON METHOD → CALL PROFILE SERVICE
        private async Task CreateProfile(string email, string fullName, string phone, string role)
        {
            try
            {
                var profileDto = new
                {
                    UserId = email, // ⚠️ TEMP (better: actual user.Id)
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Role = role
                };

                await _httpClient.PostAsJsonAsync(
                    "https://localhost:7002/api/profile/create", // 🔁 change port if needed
                    profileDto
                );
            }
            catch
            {
                // ⚠️ Do NOT fail registration if profile fails
                // Later you can log this
            }
        }
    }
}