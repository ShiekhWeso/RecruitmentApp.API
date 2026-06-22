namespace RecruitmentApp.API.DTOs
{
    public class RankTierDto
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public int MinXp { get; set; }
        public int MaxXp { get; set; }
        public string XpRange { get; set; } = string.Empty;
        public bool IsCurrentRank { get; set; }
        public bool IsUnlocked { get; set; }
    }

    public class RankResponseDto
    {
        public int TotalXp { get; set; }
        public string CurrentRank { get; set; } = string.Empty;
        public int XpToNextRank { get; set; }
        public string NextRank { get; set; } = string.Empty;
        public double ProgressPercent { get; set; }
        public List<RankTierDto> RankLadder { get; set; } = new();
        public List<XpActionDto> WaysToEarnXp { get; set; } = new();
    }

    public class XpActionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int XpReward { get; set; }
    }
}