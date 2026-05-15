using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RecruitmentApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null) throw new Exception("Email already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateAndSaveRefreshToken(user.Id);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null) throw new Exception("Invalid email or password");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid) throw new Exception("Invalid email or password");

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateAndSaveRefreshToken(user.Id);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> RefreshToken(string refreshToken)
        {
            // Step 1: Find the token in the database
            var storedToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            // Step 2: Validate it
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            // Step 3: Revoke the old refresh token (one time use)
            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            // Step 4: Generate new access token + new refresh token
            var newAccessToken = GenerateJwtToken(storedToken.User);
            var newRefreshToken = await GenerateAndSaveRefreshToken(storedToken.User.Id);

            return new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Name = storedToken.User.Name,
                Email = storedToken.User.Email,
                Role = storedToken.User.Role
            };
        }

        public async Task<bool> RevokeToken(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (storedToken == null || storedToken.IsRevoked)
                return false;

            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> GenerateAndSaveRefreshToken(Guid userId)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var refreshTokenValue = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshTokenValue;
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ForgotPassword(ForgotPasswordDto forgotpasswordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotpasswordDto.Email);
            if (user == null) throw new Exception("Email doesn't exists");

            var resetToken = Guid.NewGuid().ToString();

            var passwordResetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = resetToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            try
            {
                _context.PasswordResetTokens.Add(passwordResetToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

            return resetToken;
        }

        public async Task<string> ResetPassword(ResetPasswordDto resetpasswordDto)
        {
            var passwordResetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(prt => prt.Token == resetpasswordDto.Token);
            if (passwordResetToken == null) throw new Exception("Invalid reset token");

            if (passwordResetToken.ExpiresAt < DateTime.UtcNow) throw new Exception("Reset token has expired");
            if (passwordResetToken.IsUsed) throw new Exception("Reset token has already been used");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == passwordResetToken.UserId);
            if (user == null) throw new Exception("User not found");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetpasswordDto.NewPass);
            passwordResetToken.IsUsed = true;

            await _context.SaveChangesAsync();
            return "Password reset successful";
        }
    }
}