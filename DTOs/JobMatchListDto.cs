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
}