using LinkShortenerAPI.Data;
using LinkShortenerAPI.Models.Enitities;
using Microsoft.EntityFrameworkCore;

public interface IUrlRepository
{
    Task AddAsync(ShortenedUrl url);
    Task<ShortenedUrl> UpdateAsync(ShortenedUrl url);
    Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode);
    Task<bool> DeleteAsync(string shortCode);
}


// UrlRepository.cs (Repository Implementation)
public class UrlRepository : IUrlRepository
{
    private readonly ApplicationDbContext _context;

    public UrlRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ShortenedUrl url)
    {
        await _context.ShortenedUrls.AddAsync(url);
        await _context.SaveChangesAsync();
    }

    public async Task<ShortenedUrl> UpdateAsync(ShortenedUrl url)
    {
        if (string.IsNullOrWhiteSpace(url.ShortCode))
        {
            throw new ArgumentException("ShortCode cannot be null or empty for updating a ShortenedUrl.", nameof(url.ShortCode));
        }

        var entity = await _context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortCode == url.ShortCode);

        if (entity != null)
        {
            entity.OriginalUrl = url.OriginalUrl;
            await _context.SaveChangesAsync();
            return entity;

        }
        else
        {
            throw new KeyNotFoundException($"Shortened URL with ShortCode '{url.ShortCode}' not found.");
        }
    }

    public async Task<bool> DeleteAsync(string shortCode)
    {
        var entity = await _context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        if (entity != null)
        {
            _context.ShortenedUrls.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }
}
