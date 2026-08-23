using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IStudentService
    {
        Task<HomeScreenDto> GetHomeScreen(Guid userId);
        Task<DailyChallengeDto> GetTodaysChallenge(Guid userId);
        Task<WeeklyPlanProgressDto> GetWeeklyPlan(Guid userId);
        Task<WeeklyPlanDto> ToggleWeeklyPlanItem(Guid userId, Guid itemId);
        Task<List<StudyGroupDto>> GetStudyGroups(Guid userId);
        Task<StudyGroupDto> JoinStudyGroup(Guid userId, Guid groupId);
        Task<List<StudyGroupMessageDto>> GetGroupMessages(Guid userId, Guid groupId);
        Task<StudyGroupMessageDto> SendMessage(Guid userId, Guid groupId, SendMessageDto dto);
        Task<List<LeaderboardEntryDto>> GetWeeklyLeaderboard(Guid userId);
        Task<CareerProjectionDto> GetCareerProjection(Guid userId);
    }
}