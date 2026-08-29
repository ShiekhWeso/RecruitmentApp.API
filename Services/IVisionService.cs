using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IVisionService
    {
        Task<GapMapDto> GetGapMap(Guid userId);
        Task<HiringModeDto> GetHiringMode(Guid userId);
        Task<HiringModeDto> UpdateHiringMode(Guid userId, UpdateHiringModeDto dto);
        Task<RoadmapDto> GetRoadmap(Guid userId);
        Task<MockInterviewDto> StartMockInterview(Guid userId);
        Task<MockInterviewDto> SubmitInterviewAnswer(Guid userId, Guid interviewId, SubmitInterviewAnswerDto dto);
        Task<List<NotificationDto>> GetNotifications(Guid userId, string? type = null);
        Task<bool> MarkNotificationRead(Guid userId, Guid notificationId);
        Task<bool> MarkAllNotificationsRead(Guid userId);
        Task<NoMatchesDto> GetJobMatchStatus(Guid userId);
    }
}