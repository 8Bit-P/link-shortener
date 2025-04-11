namespace LinkShortenerAPI.Models.Enitities
{
    public class UrlAccessLog
    {
        public long Id { get; set; }
        public long UrlId { get; set; }
        public ShortenedUrl Url { get; set; } = null!;

        public DateTime AccessTime { get; set; } = DateTime.UtcNow;
        public string? Country { get; set; }
        public string? Referer { get; set; }
        public string? UserAgent { get; set; }
    }

}
