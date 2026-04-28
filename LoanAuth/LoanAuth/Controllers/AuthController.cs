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
            var (success, message, userId) = await _authService.RegisterCustomerAsync(dto);

            if (!success)
                return BadRequest(new { message });

            await CreateProfile(userId!, dto.Email, dto.FullName, dto.Phone, "Customer");

            var loginResult = await _authService.LoginAsync(new LoginDto
            {
                Email = dto.Email,
                Password = dto.Password
            });

            return Ok(new { message, userId, role = "Customer", token = loginResult.Token });
        }

        // 🔹 ADMIN REGISTER
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto dto)
        {
            var (success, message, userId) = await _authService.RegisterAdminAsync(dto);

            if (!success)
                return BadRequest(new { message });

            await CreateProfile(userId!, dto.Email, dto.FullName, dto.Phone, "Admin");

            var loginResult = await _authService.LoginAsync(new LoginDto
            {
                Email = dto.Email,
                Password = dto.Password
            });

            return Ok(new { message, userId, role = "Admin", token = loginResult.Token });
        }

        // 🔹 MANAGER REGISTER
        [HttpPost("register/manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterManagerDto dto)
        {
            var (success, message, userId) = await _authService.RegisterManagerAsync(dto);

            if (!success)
                return BadRequest(new { message });

            await CreateProfile(userId!, dto.Email, dto.FullName, dto.Phone, "Manager");

            var loginResult = await _authService.LoginAsync(new LoginDto
            {
                Email = dto.Email,
                Password = dto.Password
            });

            return Ok(new { message, userId, role = "Manager", token = loginResult.Token });
        }

        // 🔹 LOGIN (UNCHANGED)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (success, message, token, userId, role, fullName, email) = await _authService.LoginAsync(dto);
            return success
                ? Ok(new { message, token, userId, role, fullName, email })
                : Unauthorized(new { message });
        }

        private async Task CreateProfile(string userId, string email, string fullName, string phone, string role)
        {
            try
            {
                var profileDto = new
                {
                    UserId = userId,
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Role = role
                };

                await _httpClient.PostAsJsonAsync(
                    "http://localhost:5260/api/profile/create",
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
