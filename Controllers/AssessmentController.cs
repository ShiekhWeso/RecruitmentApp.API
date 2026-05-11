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
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController (IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartAssessment(StartAssessmentDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _assessmentService.StartAssessment(userId, dto);
                return Ok(new { message = "Assessment started", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{sessionId}/submit")]
        public async Task<IActionResult> SubmitAssessment(Guid sessionId, SubmitAssessmentDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _assessmentService.SubmitAssessment(userId, sessionId, dto);
                return Ok(new { message = "Assessment submitted", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}