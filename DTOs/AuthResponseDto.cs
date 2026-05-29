using Microsoft.Identity.Client;

namespace RecruitmentApp.API.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public bool OnboardingComplete { get; set; }
        public bool HasCv { get; set; }
    }
}