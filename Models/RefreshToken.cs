namespace RecruitmentApp.API.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}