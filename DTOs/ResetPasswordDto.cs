namespace RecruitmentApp.API.DTOs
{
    public class ResetPasswordDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPass { get; set; } = string.Empty;
    }
}