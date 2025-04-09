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
    public async Task<ActionResult<ShortenedUrl>> CreateShortUrl([FromBody] UrlRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var shortUrl = await _urlService.ShortenUrl(request);

        return CreatedAtAction(nameof(RedirectToOriginal), new { shortCode = shortUrl.ShortCode }, shortUrl);
    }


    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var result = await _urlService.GetUrlByShortCode(shortCode);
        if (result == null)
            return NotFound();

        return Redirect(result.OriginalUrl);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateShortUrl([FromBody] UpdateUrlRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _urlService.UpdateShortUrl(request);

        return Ok(result);
    }

    [HttpDelete("{shortCode}")]
    public async Task<IActionResult> DeleteShortUrl(string shortCode)
    {
        var success = await _urlService.DeleteShortenedUrl(shortCode);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
