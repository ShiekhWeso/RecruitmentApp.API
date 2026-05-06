using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.API.Services;
using System.Security.Claims;

namespace RecruitmentApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController (IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null) return Unauthorized(new { message = "Invalid Token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _dashboardService.GetDashboard(userId);
                return Ok(new { message = "Dashboard retrieved", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { meesage = ex.Message });
            }
        }
    }
}
