using Azure.Core;
using LinkShortenerAPI.Models.DTOs;
using LinkShortenerAPI.Models.Enitities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UrlController : ControllerBase
{
    private readonly ILogger<UrlController> _logger;
    private readonly IUrlService _urlService; 

    public UrlController(ILogger<UrlController> logger, IUrlService urlService)
    {
        _logger = logger;
        _urlService = urlService;
    }



    [HttpPost]
    public ActionResult<ShortenedUrl> CreateShortUrl([FromBody] UrlRequest request)
    {
        var shortUrl = _urlService.ShortenUrl(request);

        return CreatedAtAction(nameof(GetByShortCode), new { shortCode = shortUrl.ShortCode }, shortUrl);
    }


    [HttpGet("{shortCode}")]
    public ActionResult<ShortenedUrl> GetByShortCode(string shortCode)
    {
        var shortUrl = _urlService.GetUrlByShortCode(shortCode);

        return Ok(shortUrl);
    }
}
