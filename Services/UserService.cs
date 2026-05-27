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
                Fields = new List<string>
        {
            "Software Dev",
            "Marketing",
            "Design",
            "Finance",
            "Human Resources",
            "Sales"
        },
                Specializations = new Dictionary<string, List<string>>
        {
            { "Software Dev", new List<string> { "Frontend Developer", "Backend Developer", "Full Stack Developer", "Mobile Developer", "DevOps Engineer", "Data Scientist" } },
            { "Marketing", new List<string> { "Digital Marketing", "Content Marketing", "SEO Specialist", "Social Media Manager", "Brand Manager" } },
            { "Design", new List<string> { "UI/UX Designer", "Graphic Designer", "Motion Designer", "Product Designer" } },
            { "Finance", new List<string> { "Financial Analyst", "Accountant", "Investment Banker", "Risk Analyst" } },
            { "Human Resources", new List<string> { "HR Generalist", "Recruiter", "Training Specialist", "Compensation Analyst" } },
            { "Sales", new List<string> { "Sales Representative", "Account Manager", "Business Development", "Sales Manager" } }
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
    }
}