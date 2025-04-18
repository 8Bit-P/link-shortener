namespace LinkShortenerAPI.Models.DTOs
{
    public class StatsDTO
    {
        public long totalClicks;
        public ReferrerDTO[] Referrers { get; set; } = [];
        public LocationDTO[] Locations { get; set; } = [];
        public ClicksDTO[] Clicks { get; set; } = [];
    }

    public class ReferrerDTO
    {
        public required string Referrer { get; set; }
        public required long Clicks { get; set; }
    }

    public class LocationDTO
    {
        public required string Country { get; set; }
        public required long Clicks { get; set; }
    }

    public class ClicksDTO
    {
        public required string DayOfWeek { get; set; }
        public required long Clicks { get; set; }
    }
}
