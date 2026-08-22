using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class GamificationService : IGamificationService
    {
        private readonly AppDbContext _context;

        public GamificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GamificationProfileDto> GetGamificationProfile(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            await CheckAndAwardBadges(userId);

            var completedAssessments = await _context.Assessments
                .CountAsync(a => a.UserId == userId && a.Status == "completed");
            var completedCourses = await _context.Enrollments
                .CountAsync(e => e.UserId == userId && e.IsCompleted);
            var totalXp = (completedAssessments * 100) + (user.HasCv ? 50 : 0) + (completedCourses * 200);
            var level = (totalXp / 500) + 1;

            var rankTiers = new[]
            {
                new { Name = "Scout", Min = 0 },
                new { Name = "Rising Talent", Min = 1000 },
                new { Name = "Verified Candidate", Min = 5000 },
                new { Name = "Expert Talent", Min = 10000 },
                new { Name = "Master", Min = 25000 }
            };
            var currentRank = rankTiers.Last(r => totalXp >= r.Min).Name;
            var topPercentile = totalXp >= 10000 ? "Top 5%" : totalXp >= 5000 ? "Top 15%" : "Top 30%";

            var userBadges = await _context.UserBadges
                .Include(ub => ub.Badge)
                .Where(ub => ub.UserId == userId)
                .ToListAsync();

            var milestones = await _context.Milestones
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return new GamificationProfileDto
            {
                Name = user.Name,
                Specialization = user.Specialization ?? "Not set",
                TotalXp = totalXp,
                CurrentRank = currentRank,
                TopPercentile = topPercentile,
                Level = level,
                Badges = userBadges.Select(ub => new UserBadgeDto
                {
                    BadgeId = ub.BadgeId,
                    Name = ub.Badge.Name,
                    Description = ub.Badge.Description,
                    Icon = ub.Badge.Icon,
                    XpReward = ub.Badge.XpReward,
                    IsEarned = ub.IsEarned,
                    Progress = ub.Progress,
                    RequirementValue = ub.Badge.RequirementValue,
                    EarnedAt = ub.EarnedAt
                }).ToList(),
                Milestones = milestones.Select(m => new MilestoneDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    IsCompleted = m.IsCompleted,
                    CompletedAt = m.CompletedAt
                }).ToList()
            };
        }

        public async Task CheckAndAwardBadges(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return;

            var completedAssessments = await _context.Assessments
                .CountAsync(a => a.UserId == userId && a.Status == "completed");
            var completedCourses = await _context.Enrollments
                .CountAsync(e => e.UserId == userId && e.IsCompleted);
            var applications = await _context.JobApplications
                .CountAsync(a => a.UserId == userId);

            var allBadges = await _context.Badges.ToListAsync();

            foreach (var badge in allBadges)
            {
                var userBadge = await _context.UserBadges
                    .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BadgeId == badge.Id);

                int currentProgress = badge.RequirementType switch
                {
                    "assessments_completed" => completedAssessments,
                    "courses_completed" => completedCourses,
                    "jobs_applied" => applications,
                    "cv_uploaded" => user.HasCv ? 1 : 0,
                    _ => 0
                };

                if (userBadge == null)
                {
                    userBadge = new UserBadge
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        BadgeId = badge.Id,
                        Progress = currentProgress,
                        IsEarned = currentProgress >= badge.RequirementValue,
                        EarnedAt = currentProgress >= badge.RequirementValue ? DateTime.UtcNow : null
                    };
                    _context.UserBadges.Add(userBadge);
                }
                else if (!userBadge.IsEarned)
                {
                    userBadge.Progress = currentProgress;
                    if (currentProgress >= badge.RequirementValue)
                    {
                        userBadge.IsEarned = true;
                        userBadge.EarnedAt = DateTime.UtcNow;
                        user.XP += badge.XpReward;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}