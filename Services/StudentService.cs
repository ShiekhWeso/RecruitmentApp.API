using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Data;
using RecruitmentApp.API.DTOs;
using RecruitmentApp.API.Models;

namespace RecruitmentApp.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HomeScreenDto> GetHomeScreen(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var analysis = await _context.CvAnalyses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AnalyzedAt)
                .FirstOrDefaultAsync();

            var completedAssessments = await _context.Assessments
                .CountAsync(a => a.UserId == userId && a.Status == "completed");
            var completedCourses = await _context.Enrollments
                .CountAsync(e => e.UserId == userId && e.IsCompleted);
            var totalXp = (completedAssessments * 100) + (user.HasCv ? 50 : 0) + (completedCourses * 200);

            var currentRank = totalXp >= 25000 ? "Master" :
                              totalXp >= 10000 ? "Expert Talent" :
                              totalXp >= 5000 ? "Verified Candidate" :
                              totalXp >= 1000 ? "Rising Talent" : "Scout";

            var myGroup = await _context.StudyGroupMembers
                .Include(m => m.StudyGroup)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            var weeklyPlan = await GetWeeklyPlan(userId);
            var todaysChallenge = await GetTodaysChallenge(userId);

            return new HomeScreenDto
            {
                Name = user.Name,
                SkillScore = analysis?.Score ?? 0,
                CurrentRank = currentRank,
                XpThisWeek = completedAssessments * 10,
                TodaysChallenge = todaysChallenge,
                WeeklyPlan = weeklyPlan,
                MyStudyGroup = myGroup == null ? null : new StudyGroupDto
                {
                    Id = myGroup.StudyGroup.Id,
                    Name = myGroup.StudyGroup.Name,
                    Field = myGroup.StudyGroup.Field,
                    MemberCount = myGroup.StudyGroup.MemberCount,
                    ActiveCount = myGroup.StudyGroup.ActiveCount,
                    IsMember = true
                }
            };
        }

        public async Task<DailyChallengeDto> GetTodaysChallenge(Guid userId)
        {
            var today = DateTime.UtcNow.Date;
            var challenge = await _context.DailyChallenges
                .FirstOrDefaultAsync(c => c.ChallengeDate == today && c.IsActive);

            if (challenge == null)
            {
                challenge = new DailyChallenge
                {
                    Id = Guid.NewGuid(),
                    Title = "Marketing Quiz #14",
                    Field = "Marketing",
                    QuestionCount = 10,
                    EstimatedMinutes = 8,
                    StudentsDoneToday = 63,
                    ChallengeDate = today,
                    IsActive = true
                };
                _context.DailyChallenges.Add(challenge);
                await _context.SaveChangesAsync();
            }

            return new DailyChallengeDto
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Field = challenge.Field,
                QuestionCount = challenge.QuestionCount,
                EstimatedMinutes = challenge.EstimatedMinutes,
                StudentsDoneToday = challenge.StudentsDoneToday
            };
        }

        public async Task<WeeklyPlanProgressDto> GetWeeklyPlan(Guid userId)
        {
            var now = DateTime.UtcNow;
            var weekNumber = System.Globalization.ISOWeek.GetWeekOfYear(now);

            var items = await _context.WeeklyPlans
                .Where(w => w.UserId == userId && w.WeekNumber == weekNumber && w.Year == now.Year)
                .ToListAsync();

            if (!items.Any())
            {
                var defaultItems = new List<string> { "Chapter 4", "Chapter 5", "Chapter 6", "Weekly quiz" };
                foreach (var title in defaultItems)
                {
                    items.Add(new WeeklyPlan
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Title = title,
                        IsCompleted = false,
                        WeekNumber = weekNumber,
                        Year = now.Year
                    });
                }
                _context.WeeklyPlans.AddRange(items);
                await _context.SaveChangesAsync();
            }

            var completedCount = items.Count(i => i.IsCompleted);
            var progressPercent = items.Any() ? (int)Math.Round((double)completedCount / items.Count * 100) : 0;

            return new WeeklyPlanProgressDto
            {
                ProgressPercent = progressPercent,
                Items = items.Select(i => new WeeklyPlanDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    IsCompleted = i.IsCompleted
                }).ToList()
            };
        }

        public async Task<WeeklyPlanDto> ToggleWeeklyPlanItem(Guid userId, Guid itemId)
        {
            var item = await _context.WeeklyPlans
                .FirstOrDefaultAsync(w => w.Id == itemId && w.UserId == userId);
            if (item == null) throw new Exception("Plan item not found");

            item.IsCompleted = !item.IsCompleted;
            await _context.SaveChangesAsync();

            return new WeeklyPlanDto
            {
                Id = item.Id,
                Title = item.Title,
                IsCompleted = item.IsCompleted
            };
        }

        public async Task<List<StudyGroupDto>> GetStudyGroups(Guid userId)
        {
            var groups = await _context.StudyGroups.ToListAsync();
            var myMemberships = await _context.StudyGroupMembers
                .Where(m => m.UserId == userId)
                .Select(m => m.StudyGroupId)
                .ToListAsync();

            return groups.Select(g => new StudyGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Field = g.Field,
                Description = g.Description,
                MemberCount = g.MemberCount,
                ActiveCount = g.ActiveCount,
                IsMember = myMemberships.Contains(g.Id)
            }).ToList();
        }

        public async Task<StudyGroupDto> JoinStudyGroup(Guid userId, Guid groupId)
        {
            var group = await _context.StudyGroups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null) throw new Exception("Study group not found");

            var existing = await _context.StudyGroupMembers
                .FirstOrDefaultAsync(m => m.UserId == userId && m.StudyGroupId == groupId);
            if (existing != null) throw new Exception("Already a member");

            _context.StudyGroupMembers.Add(new StudyGroupMember
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                StudyGroupId = groupId,
                JoinedAt = DateTime.UtcNow
            });

            group.MemberCount++;
            await _context.SaveChangesAsync();

            return new StudyGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Field = group.Field,
                Description = group.Description,
                MemberCount = group.MemberCount,
                ActiveCount = group.ActiveCount,
                IsMember = true
            };
        }

        public async Task<List<StudyGroupMessageDto>> GetGroupMessages(Guid userId, Guid groupId)
        {
            var isMember = await _context.StudyGroupMembers
                .AnyAsync(m => m.UserId == userId && m.StudyGroupId == groupId);
            if (!isMember) throw new Exception("Not a member of this group");

            var messages = await _context.StudyGroupMessages
                .Include(m => m.User)
                .Where(m => m.StudyGroupId == groupId)
                .OrderBy(m => m.SentAt)
                .Take(50)
                .ToListAsync();

            return messages.Select(m => new StudyGroupMessageDto
            {
                Id = m.Id,
                SenderName = m.User.Name,
                Message = m.Message,
                SentAt = m.SentAt,
                IsMe = m.UserId == userId
            }).ToList();
        }

        public async Task<StudyGroupMessageDto> SendMessage(Guid userId, Guid groupId, SendMessageDto dto)
        {
            var isMember = await _context.StudyGroupMembers
                .AnyAsync(m => m.UserId == userId && m.StudyGroupId == groupId);
            if (!isMember) throw new Exception("Not a member of this group");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var message = new StudyGroupMessage
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                StudyGroupId = groupId,
                Message = dto.Message,
                SentAt = DateTime.UtcNow
            };

            _context.StudyGroupMessages.Add(message);
            await _context.SaveChangesAsync();

            return new StudyGroupMessageDto
            {
                Id = message.Id,
                SenderName = user!.Name,
                Message = message.Message,
                SentAt = message.SentAt,
                IsMe = true
            };
        }

        public async Task<List<LeaderboardEntryDto>> GetWeeklyLeaderboard(Guid userId)
        {
            var users = await _context.Users
                .Where(u => !u.IsDeleted)
                .Take(10)
                .ToListAsync();

            var leaderboard = new List<LeaderboardEntryDto>();
            var random = new Random();
            int rank = 1;

            foreach (var u in users.OrderByDescending(u => u.XP))
            {
                leaderboard.Add(new LeaderboardEntryDto
                {
                    Rank = rank++,
                    Name = u.Name,
                    Points = u.XP > 0 ? u.XP : random.Next(80, 160),
                    IsMe = u.Id == userId
                });
            }

            return leaderboard;
        }

        public async Task<CareerProjectionDto> GetCareerProjection(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var completedAssessments = await _context.Assessments
                .CountAsync(a => a.UserId == userId && a.Status == "completed");
            var completedCourses = await _context.Enrollments
                .CountAsync(e => e.UserId == userId && e.IsCompleted);
            var totalXp = (completedAssessments * 100) + (user.HasCv ? 50 : 0) + (completedCourses * 200);

            var currentRank = totalXp >= 25000 ? "Master" :
                              totalXp >= 10000 ? "Expert Talent" :
                              totalXp >= 5000 ? "Verified Candidate" :
                              totalXp >= 1000 ? "Rising Talent" : "Scout";

            var jobMatchesToday = await _context.Jobs.CountAsync(j => j.IsActive);

            return new CareerProjectionDto
            {
                CurrentRank = currentRank,
                ProjectedRank = "Senior Talent",
                CurrentXp = totalXp,
                JobMatchesToday = jobMatchesToday,
                JobMatchesAtProjection = jobMatchesToday * 3,
                ToGetThere = new List<string>
                {
                    "3 more skill tests",
                    "2 courses + re-tests",
                    "30-day streak",
                    "Join 2 study groups"
                }
            };
        }
    }
}