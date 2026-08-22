using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.API.Services;
using System.Security.Claims;

namespace RecruitmentApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GamificationController : ControllerBase
    {
        private readonly IGamificationService _gamificationService;

        public GamificationController(IGamificationService gamificationService)
        {
            _gamificationService = gamificationService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetGamificationProfile()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _gamificationService.GetGamificationProfile(userId);
                return Ok(new { message = "Gamification profile retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}