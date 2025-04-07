using System.Collections.Generic;
using LinkShortenerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortenerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        public DbSet<UrlAccessLog> UrlAccessLogs{ get; set; }
    }
}
