using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IGamificationService
    {
        Task<GamificationProfileDto> GetGamificationProfile(Guid userId);
        Task CheckAndAwardBadges(Guid userId);
    }
}