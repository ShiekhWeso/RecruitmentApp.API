using RecruitmentApp.API.DTOs;

public interface ICourseService
{
    Task<LearningPathDto> GetRecommendations(Guid userId);
    Task<List<CourseListDto>> GetCourses(Guid userId, string? field = null);
    Task<CourseDetailDto> GetCourseDetail(Guid courseId, Guid userId);
    Task<EnrollmentDto> EnrollInCourse(Guid userId, Guid courseId);
    Task<List<EnrollmentDto>> GetMyEnrollments(Guid userId);
    Task<EnrollmentDto> UpdateProgress(Guid userId, Guid enrollmentId, UpdateProgressDto dto);
}