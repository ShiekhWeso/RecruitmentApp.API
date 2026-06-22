using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IRankService
    {
        Task<RankResponseDto> GetRank(Guid userId);
    }
}