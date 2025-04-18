using System.Text.Json.Serialization;

namespace LinkShortenerAPI.Models
{
    public class IpInfo
    {
        [JsonPropertyName("query")]
        public string Ip { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("regionName")]
        public string Region { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("org")]
        public string Org { get; set; }

        [JsonPropertyName("zip")]
        public string Postal { get; set; }

        public string Loc => $"{Latitude},{Longitude}";
    }
}
