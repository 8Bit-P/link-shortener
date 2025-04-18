// IUrlService.cs (Service Interface)
using System.Globalization;
using LinkShortenerAPI.Models;
using LinkShortenerAPI.Models.DTOs;
using LinkShortenerAPI.Models.Enitities;
using LinkShortenerAPI.Services.Utils;
using Newtonsoft.Json;

public interface IUrlService
{
    Task<ShortenedUrlDTO> ShortenUrl(UrlRequest request);
    Task<UrlRequest?> GetUrlByShortCode(string shortCode, string? country, string? referer, string? userAgent);
    Task<ShortenedUrlDTO?> UpdateShortUrl(UpdateUrlRequest request);
    Task<bool> DeleteShortenedUrl(string shortCode);
    Task<string> GetUserCountryByIpAsync(string? ip);
    Task<StatsDTO> GetStats(string shortCode);
}

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;
    private static readonly HttpClient client = new HttpClient();

    public UrlService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    /// <summary>
    /// Creates a shortened URL entry in the DB and returns it 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ShortenedUrlDTO> ShortenUrl(UrlRequest request)
    {   
        //Create simple object
        var newShortenedUrl = new ShortenedUrl
        {
            OriginalUrl = request.OriginalURL,
            UpdatedAt = DateTime.UtcNow
        };

        //Add it to get its ID on the DB
        await _urlRepository.AddAsync(newShortenedUrl);  

        //Create shortcode based on generated id from DB
        var shortenedUrlID = newShortenedUrl.Id;
        var shortCode = Base62Encoder.Encode(shortenedUrlID);

        newShortenedUrl.ShortCode = shortCode;

        //Update it with the new shortcode
        await _urlRepository.UpdateAsync(newShortenedUrl);

        var shortenedUrlDTO = new ShortenedUrlDTO
        {
            ShortCode = newShortenedUrl.ShortCode,
            OriginalUrl = newShortenedUrl.OriginalUrl,
            Stats = getStatsFromShortenURL(newShortenedUrl)
        };

        return shortenedUrlDTO;
    }

    public async Task<UrlRequest?> GetUrlByShortCode(string shortCode, string? country, string? referer, string? userAgent)
    {   
        var shortenedUrl = await _urlRepository.GetByShortCodeAsync(shortCode);

        //if object is null throw an error 
        if (shortenedUrl == null) return null;

        //Update url stats
        await updateUrlStats(shortenedUrl, country, referer, userAgent);

        var url = new UrlRequest { OriginalURL = shortenedUrl.OriginalUrl };

        return url;
    }


    public async Task<ShortenedUrlDTO?> UpdateShortUrl(UpdateUrlRequest request)
    {
        var shortenedUrl = new ShortenedUrl()
        {
            ShortCode = request.ShortCode,
            UpdatedAt = DateTime.UtcNow,
            OriginalUrl = request.NewURL
        };

        var result = await _urlRepository.UpdateUrlAsync(shortenedUrl);

        if(result == null) return null;

        var shortenedUrlDTO = new ShortenedUrlDTO
        {
            ShortCode = result.ShortCode!,
            OriginalUrl = result.OriginalUrl,
            Stats = getStatsFromShortenURL(result)
        };

        return shortenedUrlDTO;
    }

    /// <summary>
    /// Deletes shortURL record from database 
    /// </summary>
    /// <param name="shortCode"></param>
    /// <returns></returns>
    public async Task<bool> DeleteShortenedUrl(string shortCode)
    {
        return await _urlRepository.DeleteAsync(shortCode);
    }

    public async Task<string> GetUserCountryByIpAsync(string? ip)
    {
        if (ip == null) return "";

        var ipInfo = new IpInfo();

        try
        {
            // Send the request to ipinfo.io and get the response as a string
            string info = await client.GetStringAsync("http://ip-api.com/json/" + ip);

            // Deserialize the response to IpInfo object
            ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);

            // Use RegionInfo to get the country name in English
            if (!string.IsNullOrEmpty(ipInfo?.Country))
            {
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName; // Gets the English name of the country
            }
        }
        catch (Exception)
        {
            return "";
        }

        return ipInfo.Country;
    }

    /// <summary>
    /// Gets the StatsDTO object associated to a ShortenedUrl
    /// </summary>
    /// <param name="shortenedUrl"></param>
    /// <returns></returns>
    private StatsDTO getStatsFromShortenURL(ShortenedUrl shortenedUrl)
    {
        // Handling null AccessLogs scenario, to avoid null reference exceptions
        if (shortenedUrl.AccessLogs == null || !shortenedUrl.AccessLogs.Any())
        {
            return new StatsDTO
            {   
                totalClicks = shortenedUrl.AccessCount,
                Referrers = [],
                Locations = [],
                Clicks = [],
            };
        }

        // Grouping by referer and counting the occurrences
        var referrers = shortenedUrl.AccessLogs
            .Where(x => x.Referer != null) // Only consider logs with a referer
            .GroupBy(x => x.Referer) // Group by referer
            .Select(g => new ReferrerDTO
            {
                Referrer = g.Key,
                Clicks = g.Count() // Count the occurrences of each referer
            })
            .ToArray();

        // Grouping by country and counting the occurrences
        var locations = shortenedUrl.AccessLogs
            .Where(x => x.Country != null) // Only consider logs with a country
            .GroupBy(x => x.Country) // Group by country
            .Select(g => new LocationDTO
            {
                Country = g.Key,
                Clicks = g.Count() // Count the occurrences of each country
            })
            .ToArray();

        // Grouping by day of the week and counting the occurrences
        var clicks = shortenedUrl.AccessLogs
            .GroupBy(x => x.AccessTime.DayOfWeek) // Group by day of the week
            .Select(g => new ClicksDTO
            {
                DayOfWeek = g.Key.ToString(), // Get the string representation of the DayOfWeek (e.g., Sunday, Monday, etc.)
                Clicks = g.Count() // Count the occurrences of each day of the week
            })
            .ToArray();

        // Return the StatsDTO with the populated data
        return new StatsDTO
        {
            Referrers = referrers,
            Locations = locations,
            Clicks = clicks
        };
    }

    public async Task<StatsDTO?> GetStats(string shortCode)
    {
        var shortenedUrl = await _urlRepository.GetByShortCodeAsync(shortCode);

        if (shortenedUrl == null) return null;

        return getStatsFromShortenURL(shortenedUrl);

    }

    private async Task updateUrlStats(ShortenedUrl shortenedUrl, string? country, string? referer, string? userAgent)
    {
        // Increment the access count and add the log
        shortenedUrl.AccessCount++;

        if (shortenedUrl.AccessLogs == null)
        {
            shortenedUrl.AccessLogs = new List<UrlAccessLog>();
        }

        shortenedUrl.AccessLogs.Add(new UrlAccessLog
        {
            UrlId = shortenedUrl.Id,
            AccessTime = DateTime.UtcNow,
            Country = country,
            Referer = referer,
            UserAgent = userAgent
        });

        // Update the URL stats in the repository
        await _urlRepository.UpdateUrlAsync(shortenedUrl);
    }
}
