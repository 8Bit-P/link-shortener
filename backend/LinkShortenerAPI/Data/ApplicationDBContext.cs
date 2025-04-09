using System.Collections.Generic;
using LinkShortenerAPI.Models.Enitities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure ShortCode is unique using Fluent API
            modelBuilder.Entity<ShortenedUrl>()
                .HasIndex(u => u.ShortCode)
                .IsUnique();
        }
    }
}
