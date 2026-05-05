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
    }
}