using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Services;
using System.Security.Claims;

namespace RecruitmentApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _userService.GetProfile(userId);
                return Ok(new { message = "Profile retrieved", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("setup")]
        public async Task<IActionResult> UpdateSetup(SetupDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _userService.UpdateSetup(userId, dto);
                return Ok(new { message = "Setup complete", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _userService.GetProfile(userId);
                return Ok(new { message = "User retrieved", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}