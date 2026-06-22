using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.API.Services;
using System.Security.Claims;

namespace RecruitmentApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RankController : ControllerBase
    {
        private readonly IRankService _rankService;

        public RankController(IRankService rankService)
        {
            _rankService = rankService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRank()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _rankService.GetRank(userId);
                return Ok(new { message = "Rank retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}