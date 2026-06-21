namespace RecruitmentApp.API.DTOs
{
    public class JobApplicationDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int MatchPercentage { get; set; }
        public DateTime AppliedAt { get; set; }
    }

    public class ApplyJobDto
    {
        public Guid JobId { get; set; }
    }
}