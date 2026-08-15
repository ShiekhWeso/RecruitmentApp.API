using Microsoft.Identity.Client;

namespace RecruitmentApp.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Field { get; set; }
        public string? Specialization { get; set; }
        public string? ExperienceLevel { get; set; }
        public bool OnboardingComplete { get; set; } = false;
        public bool HasCv { get; set; } = false;
        public string? Locatoin { get; set; }
        public int XP { get; set; } = 0;
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IncognitoMode { get; set; } = false;
        public bool ProfileVisible { get; set; } = true;
        public bool PushNotifications { get; set; } = true;
        public string Language { get; set; } = "English";
        public string Region { get; set; } = "Egypt";
        public string SubscriptionPlan { get; set; } = "Free";
        public bool IsDeleted { get; set; } = false;
    }
}   