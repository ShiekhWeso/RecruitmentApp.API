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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("home")]
        public async Task<IActionResult> GetHomeScreen()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetHomeScreen(userId);
                return Ok(new { message = "Home screen retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("daily-challenge")]
        public async Task<IActionResult> GetTodaysChallenge()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetTodaysChallenge(userId);
                return Ok(new { message = "Daily challenge retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("weekly-plan")]
        public async Task<IActionResult> GetWeeklyPlan()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetWeeklyPlan(userId);
                return Ok(new { message = "Weekly plan retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("weekly-plan/{itemId}/toggle")]
        public async Task<IActionResult> ToggleWeeklyPlanItem(Guid itemId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.ToggleWeeklyPlanItem(userId, itemId);
                return Ok(new { message = "Plan item updated", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("study-groups")]
        public async Task<IActionResult> GetStudyGroups()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetStudyGroups(userId);
                return Ok(new { message = "Study groups retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("study-groups/{groupId}/join")]
        public async Task<IActionResult> JoinStudyGroup(Guid groupId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.JoinStudyGroup(userId, groupId);
                return Ok(new { message = "Joined study group", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("study-groups/{groupId}/messages")]
        public async Task<IActionResult> GetGroupMessages(Guid groupId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetGroupMessages(userId, groupId);
                return Ok(new { message = "Messages retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("study-groups/{groupId}/messages")]
        public async Task<IActionResult> SendMessage(Guid groupId, SendMessageDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.SendMessage(userId, groupId, dto);
                return Ok(new { message = "Message sent", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetWeeklyLeaderboard()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetWeeklyLeaderboard(userId);
                return Ok(new { message = "Leaderboard retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("career-projection")]
        public async Task<IActionResult> GetCareerProjection()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _studentService.GetCareerProjection(userId);
                return Ok(new { message = "Career projection retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}