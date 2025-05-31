namespace LinkShortenerAPI.Models.DTOs
{
    public class UpdateUrlRequest
    {
        public required string ShortCode { get; set; }
        public required string NewURL { get; set; }
    }
}
