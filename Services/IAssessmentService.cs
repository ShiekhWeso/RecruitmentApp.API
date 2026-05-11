using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IAssessmentService
    {
        Task<AssessmentSessionDto> StartAssessment(Guid userId, StartAssessmentDto dto);
        Task<AssessmentResultDto> SubmitAssessment(Guid userId, Guid sessionId, SubmitAssessmentDto dto);
    }
}