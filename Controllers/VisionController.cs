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
    public class VisionController : ControllerBase
    {
        private readonly IVisionService _visionService;

        public VisionController(IVisionService visionService)
        {
            _visionService = visionService;
        }

        [HttpGet("gap-map")]
        public async Task<IActionResult> GetGapMap()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.GetGapMap(userId);
                return Ok(new { message = "Gap map retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("hiring-mode")]
        public async Task<IActionResult> GetHiringMode()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.GetHiringMode(userId);
                return Ok(new { message = "Hiring mode retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("hiring-mode")]
        public async Task<IActionResult> UpdateHiringMode(UpdateHiringModeDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.UpdateHiringMode(userId, dto);
                return Ok(new { message = "Hiring mode updated", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("roadmap")]
        public async Task<IActionResult> GetRoadmap()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.GetRoadmap(userId);
                return Ok(new { message = "Roadmap retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("mock-interview/start")]
        public async Task<IActionResult> StartMockInterview()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.StartMockInterview(userId);
                return Ok(new { message = "Mock interview started", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("mock-interview/{interviewId}/answer")]
        public async Task<IActionResult> SubmitInterviewAnswer(Guid interviewId, SubmitInterviewAnswerDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.SubmitInterviewAnswer(userId, interviewId, dto);
                return Ok(new { message = "Answer submitted", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] string? type)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.GetNotifications(userId, type);
                return Ok(new { message = "Notifications retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("notifications/{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationRead(Guid notificationId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await _visionService.MarkNotificationRead(userId, notificationId);
                return Ok(new { message = "Notification marked as read" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("notifications/read-all")]
        public async Task<IActionResult> MarkAllNotificationsRead()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await _visionService.MarkAllNotificationsRead(userId);
                return Ok(new { message = "All notifications marked as read" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("job-match-status")]
        public async Task<IActionResult> GetJobMatchStatus()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _visionService.GetJobMatchStatus(userId);
                return Ok(new { message = "Job match status retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}