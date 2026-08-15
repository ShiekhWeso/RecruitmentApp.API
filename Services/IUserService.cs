using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfile(Guid userId);
        Task<UserProfileDto> UpdateSetup(Guid userId, SetupDto dto);
        Task<UserProfileDto> UpdateProfile(Guid userId, UpdateProfileDto dto);
        Task<SetupOptionsDto> GetSetupOptions();

        Task<SettingsResponseDto> GetSettings(Guid userId);
        Task<SettingsResponseDto> UpdatePersonalInfo(Guid userId, UpdatePersonalInfoDto dto);

        Task<bool> ChangePassword(Guid userId, ChangePasswordDto dto);
        Task<SettingsResponseDto> UpdatePrivacy(Guid userId, UpdatePrivacyDto dto);
        Task<SettingsResponseDto> UpdateNotifications(Guid userId, UpdateNotificationsDto dto);
        Task<SettingsResponseDto> UpdateLanguage(Guid userId, UpdateLanguageDto dto);
        Task<SubscriptionPlanDto> GetSubscriptionPlans(Guid userId);
        Task<bool> UpgradeToPro(Guid userId);
        Task<bool> DeleteAccount(Guid userId);
    }
}