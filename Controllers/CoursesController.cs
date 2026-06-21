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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Invalid token" });

                var userId = Guid.Parse(userIdClaim);
                var result = await _courseService.GetRecommendations(userId);
                return Ok(new { message = "Recommendations retrieved", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] string? field)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _courseService.GetCourses(userId, field);
                return Ok(new { message = "Courses retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseDetail(Guid courseId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _courseService.GetCourseDetail(courseId, userId);
                return Ok(new { message = "Course retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("{courseId}/enroll")]
        public async Task<IActionResult> Enroll(Guid courseId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _courseService.EnrollInCourse(userId, courseId);
                return Ok(new { message = "Enrolled successfully", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _courseService.GetMyEnrollments(userId);
                return Ok(new { message = "Enrollments retrieved", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("enrollments/{enrollmentId}/progress")]
        public async Task<IActionResult> UpdateProgress(Guid enrollmentId, UpdateProgressDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await _courseService.UpdateProgress(userId, enrollmentId, dto);
                return Ok(new { message = "Progress updated", data = result });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}