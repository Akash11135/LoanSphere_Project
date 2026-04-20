using LoanAuth.DTOs;
using LoanAuth.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanAuth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDto dto)
        {
            var (success, message, _) = await _authService.RegisterCustomerAsync(dto);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto dto)
        {
            var (success, message, _) = await _authService.RegisterAdminAsync(dto);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpPost("register/manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterManagerDto dto)
        {
            var (success, message, _) = await _authService.RegisterManagerAsync(dto);
            return success ? Ok(new { message }) : BadRequest(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (success, message, token) = await _authService.LoginAsync(dto);
            return success
                ? Ok(new { message, token })
                : Unauthorized(new { message });
        }
    }
}
