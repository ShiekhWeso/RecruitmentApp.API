using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;

namespace RecruitmentApp.API.Services
{
    public class RankService : IRankService
    {
        private readonly AppDbContext _context;

        public RankService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RankResponseDto> GetRank(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var completedAssessments = await _context.Assessments
                .CountAsync(a => a.UserId == userId && a.Status == "completed");
            var completedCourses = await _context.Enrollments
                .CountAsync(e => e.UserId == userId && e.IsCompleted);

            var totalXp = (completedAssessments * 100) + (user.HasCv ? 50 : 0) + (completedCourses * 200);

            user.XP = totalXp;
            await _context.SaveChangesAsync();

            var rankLadder = new List<RankTierDto>
            {
                new() { Title = "Scout", Subtitle = "Just getting started.", MinXp = 0, MaxXp = 999, XpRange = "0 - 999 XP" },
                new() { Title = "Rising Talent", Subtitle = "Building momentum.", MinXp = 1000, MaxXp = 4999, XpRange = "1,000 - 4,999 XP" },
                new() { Title = "Verified Candidate", Subtitle = "You are currently here.", MinXp = 5000, MaxXp = 9999, XpRange = "5,000 - 9,999 XP" },
                new() { Title = "Expert Talent", Subtitle = "Priority application reviews.", MinXp = 10000, MaxXp = 24999, XpRange = "10,000 - 24,999 XP" },
                new() { Title = "Master", Subtitle = "Elite tier opportunities.", MinXp = 25000, MaxXp = int.MaxValue, XpRange = "25,000+ XP" }
            };

            foreach (var tier in rankLadder)
            {
                tier.IsCurrentRank = totalXp >= tier.MinXp && totalXp <= tier.MaxXp;
                tier.IsUnlocked = totalXp >= tier.MinXp;
            }

            var currentTier = rankLadder.Last(t => totalXp >= t.MinXp);
            var nextTier = rankLadder.FirstOrDefault(t => totalXp < t.MinXp);
            var xpToNext = nextTier != null ? nextTier.MinXp - totalXp : 0;
            var progressPercent = nextTier != null
                ? Math.Round((double)(totalXp - currentTier.MinXp) / (nextTier.MinXp - currentTier.MinXp) * 100, 1)
                : 100;

            return new RankResponseDto
            {
                TotalXp = totalXp,
                CurrentRank = currentTier.Title,
                XpToNextRank = xpToNext,
                NextRank = nextTier?.Title ?? "Max Rank",
                ProgressPercent = progressPercent,
                RankLadder = rankLadder,
                WaysToEarnXp = new List<XpActionDto>
                {
                    new() { Title = "Complete Assessment", Description = "Take a skill verification test", XpReward = 100 },
                    new() { Title = "Upload CV", Description = "Upload your CV for AI analysis", XpReward = 50 },
                    new() { Title = "Complete Course", Description = "Finish an enrolled course", XpReward = 200 },
                    new() { Title = "Update Portfolio", Description = "Add new project links", XpReward = 20 }
                }
            };
        }
    }
}