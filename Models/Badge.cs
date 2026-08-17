namespace RecruitmentApp.API.Models
{
    public class Badge
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int XpReward { get; set; }
        public string RequirementType { get; set; } = string.Empty;
        public int RequirementValue { get; set; }
    }
}