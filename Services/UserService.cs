using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> GetProfile(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not Found");

            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();

            var assessments = await _context.Assessments.Where(a => a.UserId == userId && a.Status == "completed").OrderByDescending(a => a.CompletedAt).Take(5).ToListAsync();

            var skillBreakdown = new List<SkillBreakdownDto>();
            if (analysis != null)
            {
                var skills = System.Text.Json.JsonSerializer.Deserialize<List<string>>(analysis.Skills) ?? new();
                var random = new Random();
                skillBreakdown = skills.Select(s => new SkillBreakdownDto
                {
                    SkillName = s,
                    Score = random.Next(50, 95)
                }).ToList();
            }

            var testHistory = assessments.Select(a => new TestHistoryDto
            {
                Field = a.Field,
                Score = a.Score,
                CompletedAt = a.CompletedAt ?? DateTime.UtcNow
            }).ToList();

            return new UserProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                Location = user.Locatoin?? "Not set",
                Field = analysis?.Field ?? user.Field ?? "Not set",
                Specialization = user.Specialization ?? "Not set",
                ExperienceLevel = user.ExperienceLevel ?? "Not set",
                SkillScore = analysis?.Score ?? 0,
                IsVerified = analysis != null,
                Role = user.Role,
                SkillBreakdown = skillBreakdown,
                TestHistory = testHistory
            };
        }

        public async Task<UserProfileDto> UpdateSetup(Guid userId, SetupDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            user.Field = dto.Field;
            user.Specialization = dto.Specialization;
            user.ExperienceLevel = dto.ExperienceLevel;
            user.OnboardingComplete = true;

            await _context.SaveChangesAsync();

            return await GetProfile(userId);
        }

        public async Task<UserProfileDto> UpdateProfile(Guid userId, UpdateProfileDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Field)) user.Field = dto.Field;

            if (!string.IsNullOrEmpty(dto.Specialization)) user.Specialization = dto.Specialization;

            if (!string.IsNullOrEmpty(dto.ExperienceLevel)) user.ExperienceLevel = dto.ExperienceLevel;

            await _context.SaveChangesAsync();
            return await GetProfile(userId);
        }

        public Task<SetupOptionsDto> GetSetupOptions()
        {
            var options = new SetupOptionsDto
            {
                Fields = new List<FieldWithSpecializationsDto>
                {
                    new FieldWithSpecializationsDto { Name = "Software Dev", Specializations = new List<string> { "Frontend Developer", "Backend Developer", "Full Stack Developer", "Mobile Developer", "DevOps Engineer", "Data Scientist" } },
                    new FieldWithSpecializationsDto { Name = "Marketing", Specializations = new List<string> { "Digital Marketing", "Content Marketing", "SEO Specialist", "Social Media Manager", "Brand Manager" } },
                    new FieldWithSpecializationsDto { Name = "Design", Specializations = new List<string> { "UI/UX Designer", "Graphic Designer", "Motion Designer", "Product Designer" } },
                    new FieldWithSpecializationsDto { Name = "Finance", Specializations = new List<string> { "Financial Analyst", "Accountant", "Investment Banker", "Risk Analyst" } },
                    new FieldWithSpecializationsDto { Name = "Human Resources", Specializations = new List<string> { "HR Generalist", "Recruiter", "Training Specialist", "Compensation Analyst" } },
                    new FieldWithSpecializationsDto { Name = "Sales", Specializations = new List<string> { "Sales Representative", "Account Manager", "Business Development", "Sales Manager" } }
                },
                ExperienceLevels = new List<string>
                {
                    "Junior (0-2 years)",
                    "Mid-level (2-5 years)",
                    "Senior (5+ years)"
                }
            };
            return Task.FromResult(options);
        }

        public async Task<SettingsResponseDto> GetSettings(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User Not found");

            return MapToSettingsDto(user);
        }
        
        public async Task<SettingsResponseDto> UpdatePersonalInfo(Guid userId, UpdatePersonalInfoDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Phone)) user.Phone = dto.Phone;
            if (!string.IsNullOrEmpty(dto.AvatarUrl)) user.AvatarUrl = dto.AvatarUrl;
            if (!string.IsNullOrEmpty(dto.Location)) user.Locatoin = dto.Location;

            await _context.SaveChangesAsync();
            return MapToSettingsDto(user);
        }

        public async Task<bool> ChangePassword(Guid userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                throw new Exception("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SettingsResponseDto> UpdatePrivacy(Guid userId, UpdatePrivacyDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            if (dto.IncognitoMode.HasValue) user.IncognitoMode = dto.IncognitoMode.Value;
            if (dto.ProfileVisible.HasValue) user.ProfileVisible = dto.ProfileVisible.Value;

            await _context.SaveChangesAsync();
            return MapToSettingsDto(user);
        }

        public async Task<SettingsResponseDto> UpdateNotifications(Guid userId, UpdateNotificationsDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            if (dto.PushNotifications.HasValue) user.PushNotifications = dto.PushNotifications.Value;

            await _context.SaveChangesAsync();
            return MapToSettingsDto(user);
        }

        public async Task<SettingsResponseDto> UpdateLanguage(Guid userId, UpdateLanguageDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrEmpty(dto.Language)) user.Language = dto.Language;
            if (!string.IsNullOrEmpty(dto.Region)) user.Region = dto.Region;

            await _context.SaveChangesAsync();
            return MapToSettingsDto(user);
        }

        public async Task<SubscriptionPlanDto> GetSubscriptionPlans(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            return new SubscriptionPlanDto
            {
                CurrentPlan = user.SubscriptionPlan,
                Plans = new List<PlanOptionDto>
        {
            new()
            {
                Name = "Free",
                MonthlyPrice = 0,
                AnnualPrice = 0,
                Features = new List<string>
                {
                    "Basic skills assessment",
                    "Standard public profile",
                    "Community access"
                },
                IsRecommended = false
            },
            new()
            {
                Name = "Pro",
                MonthlyPrice = 199,
                AnnualPrice = 159,
                Features = new List<string>
                {
                    "Unlimited expert assessments",
                    "Priority ranking in search",
                    "Verified trust badge",
                    "Detailed performance analytics"
                },
                IsRecommended = true
            }
        }
            };
        }

        public async Task<bool> UpgradeToPro(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            user.SubscriptionPlan = "Pro";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccount(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private SettingsResponseDto MapToSettingsDto(Models.User user) => new()
        {
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            AvatarUrl = user.AvatarUrl,
            SubscriptionPlan = user.SubscriptionPlan,
            IncognitoMode = user.IncognitoMode,
            ProfileVisible = user.ProfileVisible,
            PushNotifications = user.PushNotifications,
            Language = user.Language,
            Region = user.Region
        };
    }
}