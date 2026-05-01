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
            // Step 1: Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null) throw new Exception("Email already exists");

            // Step 2: Create new user object
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            // Step 3: Save user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Step 4: Generate token and return response
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = "",
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            // Step 1: Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null) throw new Exception("Invalid email or password");

            // Step 2: Check password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid) throw new Exception("Invalid email or password");

            // Step 3: Generate token and return response
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = "",
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        private String GenerateJwtToken(User user)
        {
            // Step 1: Get the secret key from appsettings.json
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            // Step 2: Define the claims (info stored inside the token)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Step 3: Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"])),
                signingCredentials: credentials
                );

            // Step 4: Convert token to string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
