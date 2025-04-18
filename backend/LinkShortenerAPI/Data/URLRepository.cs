using LinkShortenerAPI.Data;
using LinkShortenerAPI.Models.Enitities;
using Microsoft.EntityFrameworkCore;

public interface IUrlRepository
{
    Task AddAsync(ShortenedUrl url);
    Task<ShortenedUrl> UpdateUrlAsync(ShortenedUrl url);
    Task UpdateAsync(ShortenedUrl url);
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

    /// <summary>
    /// Updates the Original URL of a ShortURL record
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<ShortenedUrl> UpdateUrlAsync(ShortenedUrl url)
    {
        if (string.IsNullOrWhiteSpace(url.ShortCode))
        {
            throw new ArgumentException("ShortCode cannot be null or empty for updating a ShortenedUrl.", nameof(url.ShortCode));
        }

        var entity = await _context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortCode == url.ShortCode || u.Id == url.Id);

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

    public async Task UpdateAsync(ShortenedUrl url)
    {
        _context.ShortenedUrls.Update(url);
        await _context.SaveChangesAsync();
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
        var shortenedUrl = await _context.ShortenedUrls.Include(u => u.AccessLogs).FirstOrDefaultAsync(u => u.ShortCode == shortCode);
       
        return shortenedUrl;
    }
}
