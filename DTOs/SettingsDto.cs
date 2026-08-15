namespace RecruitmentApp.API.DTOs
{
    public class SettingsResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public string SubscriptionPlan { get; set; } = string.Empty;
        public bool IncognitoMode { get; set; }
        public bool ProfileVisible { get; set; }
        public bool PushNotifications { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }

    public class UpdatePersonalInfoDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Location { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UpdatePrivacyDto
    {
        public bool? IncognitoMode { get; set; }
        public bool? ProfileVisible { get; set; }
    }

    public class UpdateNotificationsDto
    {
        public bool? PushNotifications { get; set; }
    }

    public class UpdateLanguageDto
    {
        public string? Language { get; set; }
        public string? Region { get; set; }
    }

    public class SubscriptionPlanDto
    {
        public string CurrentPlan { get; set; } = string.Empty;
        public List<PlanOptionDto> Plans { get; set; } = new();
    }

    public class PlanOptionDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public decimal AnnualPrice { get; set; }
        public List<string> Features { get; set; } = new();
        public bool IsRecommended { get; set; }
    }
}