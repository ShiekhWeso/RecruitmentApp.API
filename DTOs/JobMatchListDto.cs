namespace RecruitmentApp.API.DTOs
{
    public class JobMatchListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public int MatchPercentage { get; set; }
        public string Field { get; set; } = string.Empty;
    }

    public class JobDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public int MatchPercentage { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new();
        public List<JobSkillDto> RequiredSkills { get; set; } = new();
    }

    public class JobSkillDto
    {
        public string SkillName { get; set; } = string.Empty;
        public int NeededScore { get; set; }
        public int UserScore { get; set; }
        public bool IsMet { get; set; }
    }
}