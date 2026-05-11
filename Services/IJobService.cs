using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IJobService
    {
        Task<List<JobMatchListDto>> GetJobMatches(Guid userId);
    }
}