namespace LinkShortenerAPI.Models
{
    public class UrlAccessLog
    {
        public int Id { get; set; }
        public int UrlId { get; set; }
        public ShortenedUrl Url { get; set; } = null!;

        public DateTime AccessTime { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

}
