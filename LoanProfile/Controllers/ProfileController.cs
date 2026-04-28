using LoanProfile.DTOs;
using LoanProfile.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanProfile.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDto dto)
        {
            await _service.CreateProfileAsync(dto);
            return Ok("Profile created");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, [FromBody] UpdateProfileDto dto)
        {
            await _service.UpdateProfileAsync(userId, dto);
            return Ok("Profile updated");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var profile = await _service.GetProfileAsync(userId);
            if (profile == null) return NotFound();

            return Ok(profile);
        }
    }
}
