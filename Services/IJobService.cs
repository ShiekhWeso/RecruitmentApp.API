using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IJobService
    {
        Task<List<JobMatchListDto>> GetJobMatches(Guid userId, string? field = null, string? location = null, string? jobType = null);
        Task<JobDetailDto> GetJobDetail(Guid jobId, Guid userId);
        Task<JobApplicationDto> ApplyToJob(Guid userId, ApplyJobDto dto);
        Task<List<JobApplicationDto>> GetApplications(Guid userId, string? status = null);
        Task<bool> WithdrawApplication(Guid userId, Guid applicationId);
    }
}