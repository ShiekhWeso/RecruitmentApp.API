using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface ICvService
    {
        Task<CvUploadResponseDto> UploadCv(IFormFile file, Guid userId);
        Task<CvAnalysisResponseDto> GetCvAnalysis(Guid userId);
    }
}