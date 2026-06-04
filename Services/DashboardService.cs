using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> GetDashboard(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var analysis = await _context.CvAnalyses.Where(a => a.UserId == userId).OrderByDescending(a => a.AnalyzedAt).FirstOrDefaultAsync();

            var jobMatches = new List<JobMatchDto>
            {
                new JobMatchDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Senior Frontend Engineer",
                    Company = "TechCorp",
                    Location = "Dubai",
                    MatchPercentage = 82
                },
                new JobMatchDto
                {
                    Id = Guid.NewGuid(),
                    Title = "React Developer",
                    Company = "Innovate.io",
                    Location = "Remote",
                    MatchPercentage = 67
                }
            };

            return new DashboardResponseDto
            {
                Name = user.Name,
                Field = analysis?.Field ?? user.Field ?? "Not set",
                SkillScore = analysis?.Score ?? 0,
                IsVerified = analysis != null,
                JobMatches = jobMatches
            };
        }
    }
}