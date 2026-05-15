using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<string> ForgotPassword(ForgotPasswordDto forgotpasswordDto);
        Task<string> ResetPassword(ResetPasswordDto resetpasswordDto);
        Task<AuthResponseDto> RefreshToken(string refreshToken);
        Task<bool> RevokeToken(string refreshToken);
    }
}