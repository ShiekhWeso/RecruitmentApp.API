using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.API.Services;
using System.Security.Claims;

namespace RecruitmentApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("matches")]
        public async Task<IActionResult> GetJobMatches(
            [FromQuery] string? field,
            [FromQuery] string? location,
            [FromQuery] string? jobType)
        {
            
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null) return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _jobService.GetJobMatches(userId, field, location, jobType);
                return Ok(new { message = "Job matches retrieved", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}