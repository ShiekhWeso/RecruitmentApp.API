using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface ICourseService
    {
        Task<LearningPathDto> GetRecommendations(Guid userId);
    }
}