using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboard(Guid userId);
    }
}