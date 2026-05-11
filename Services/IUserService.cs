using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfile(Guid userId);
        Task<UserProfileDto> UpdateSetup(Guid userId, SetupDto dto);
    }
}