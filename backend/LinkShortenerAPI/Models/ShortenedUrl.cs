namespace LinkShortenerAPI.Models
{
    public class ShortenedUrl
    {
        public Guid Id { get; set; }
        public required string OriginalUrl { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public int AccessCount { get; set; } = 0;

        public List<UrlAccessLog>? AccessLogs { get; set; }
    }
}