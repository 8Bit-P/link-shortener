// IUrlService.cs (Service Interface)
using LinkShortenerAPI.Models.DTOs;
using LinkShortenerAPI.Models.Enitities;
using LinkShortenerAPI.Services.Utils;
using Microsoft.AspNetCore.DataProtection.Repositories;

public interface IUrlService
{
    ShortenedUrl ShortenUrl(UrlRequest request);
    ShortenedUrl? GetUrlByShortCode(string shortCode);
}

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;

    public UrlService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    /// <summary>
    /// Creates a shortened url register in the DB and returns it 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public ShortenedUrl ShortenUrl(UrlRequest request)
    {
        var newShortenedUrl = new ShortenedUrl
        {
            OriginalUrl = request.OriginalURL,
        };

        _urlRepository.Add(newShortenedUrl);

        var shortenedUrlID = newShortenedUrl.Id;
        var shortCode = Base62Encoder.Encode(shortenedUrlID);

        newShortenedUrl.ShortCode = shortCode;

        _urlRepository.Update(newShortenedUrl);

        return newShortenedUrl;
    }

    public ShortenedUrl? GetUrlByShortCode(string shortCode)
    {
        return _urlRepository.GetByShortCode(shortCode);
    }
}
