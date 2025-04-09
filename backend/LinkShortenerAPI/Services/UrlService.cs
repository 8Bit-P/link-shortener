// IUrlService.cs (Service Interface)
using LinkShortenerAPI.Models.DTOs;
using LinkShortenerAPI.Models.Enitities;
using LinkShortenerAPI.Services.Utils;

public interface IUrlService
{
    Task<ShortenedUrl> ShortenUrl(UrlRequest request);
    Task<ShortenedUrl?> GetUrlByShortCode(string shortCode);
    Task<ShortenedUrl?> UpdateShortUrl(UpdateUrlRequest request);
    Task<bool> DeleteShortenedUrl(string shortCode);
}

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;

    public UrlService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    /// <summary>
    /// Creates a shortened URL entry in the DB and returns it 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ShortenedUrl> ShortenUrl(UrlRequest request)
    {
        var newShortenedUrl = new ShortenedUrl
        {
            OriginalUrl = request.OriginalURL,
            UpdatedAt = DateTime.UtcNow
        };

        await _urlRepository.AddAsync(newShortenedUrl);  

        var shortenedUrlID = newShortenedUrl.Id;
        var shortCode = Base62Encoder.Encode(shortenedUrlID);

        newShortenedUrl.ShortCode = shortCode;

        await _urlRepository.UpdateAsync(newShortenedUrl);  

        return newShortenedUrl;
    }

    public async Task<ShortenedUrl?> GetUrlByShortCode(string shortCode)
    {
        return await _urlRepository.GetByShortCodeAsync(shortCode);  
    }

    public async Task<ShortenedUrl?> UpdateShortUrl(UpdateUrlRequest request)
    {
        var shortenedUrl = new ShortenedUrl()
        {
            ShortCode = request.ShortCode,
            UpdatedAt = DateTime.UtcNow,
            OriginalUrl = request.NewURL
        };

        return await _urlRepository.UpdateAsync(shortenedUrl); 
    }

    public async Task<bool> DeleteShortenedUrl(string shortCode)
    {
        return await _urlRepository.DeleteAsync(shortCode);  
    }
}
