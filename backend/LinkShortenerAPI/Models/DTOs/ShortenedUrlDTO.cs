namespace LinkShortenerAPI.Models.DTOs
{
    public class ShortenedUrlDTO
    {
        public required string ShortCode { get; set; }
        public required string OriginalUrl{ get; set; }
        public required StatsDTO Stats { get; set; }
    }
}
