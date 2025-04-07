using LinkShortenerAPI.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UrlController : ControllerBase
{
    private readonly ILogger<UrlController> _logger;

    public UrlController(ILogger<UrlController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<ShortenedUrl> CreateShortUrl([FromBody] string originalUrl)
    {
        var result = new ShortenedUrl
        {
            Id = new Guid(),
            OriginalUrl = originalUrl,
            ShortCode = "test"
        };

        return CreatedAtAction(nameof(GetByShortCode), new { shortCode = result.ShortCode }, result);
    }

    [HttpGet("{shortCode}")]
    public ActionResult<ShortenedUrl> GetByShortCode(string shortCode)
    {
        var result = new ShortenedUrl
        {
            Id = new Guid(),
            OriginalUrl = "https://example.com",
            ShortCode = shortCode
        };

        return Ok(result);
    }
}
