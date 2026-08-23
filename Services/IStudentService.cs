using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IStudentService
    {
        Task<HomeScreenDto> GetHomeScreen(Guid useId);
        Task<DailyChallengeDto> GetTodayChallenge(Guid useId);
        Task<WeeklyPlanProgressDto> GetWeeklyPlan(Guid userId);
    }
}