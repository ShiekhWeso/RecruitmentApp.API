using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentApp.API.Services;
using RecruitmentApp.API.DTOs;
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

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobDetail(Guid jobId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _jobService.GetJobDetail(jobId, userId);
                return Ok(new { message = "Job retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyToJob(ApplyJobDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _jobService.ApplyToJob(userId, dto);
                return Ok(new { message = "Application sent!", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("applications")]
        public async Task<IActionResult> GetApplications([FromQuery] string? status)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _jobService.GetApplications(userId, status);
                return Ok(new { message = "Applications retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("applications/{applicationId}")]
        public async Task<IActionResult> WithdrawApplication(Guid applicationId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await _jobService.WithdrawApplication(userId, applicationId);
                return Ok(new { message = "Application withdrawn" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}