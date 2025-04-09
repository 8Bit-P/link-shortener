using LinkShortenerAPI.Data;
using LinkShortenerAPI.Models.Enitities;

public interface IUrlRepository
{
    void Add(ShortenedUrl url);
    void Update(ShortenedUrl url);
    ShortenedUrl? GetByShortCode(string shortCode);
}

// UrlRepository.cs (Repository Implementation)
public class UrlRepository : IUrlRepository
{
    private readonly ApplicationDbContext _context;

    public UrlRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(ShortenedUrl url)
    {
        _context.ShortenedUrls.Add(url);
        _context.SaveChanges();
    }

    public void Update(ShortenedUrl url)
    {
        _context.ShortenedUrls.Update(url); 
        _context.SaveChanges();
    }

    public ShortenedUrl? GetByShortCode(string shortCode)
    {
        return _context.ShortenedUrls
            .FirstOrDefault(u => u.ShortCode == shortCode);
    }
}
