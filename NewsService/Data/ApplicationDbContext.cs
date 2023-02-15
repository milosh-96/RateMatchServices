using Microsoft.EntityFrameworkCore;

namespace NewsService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NewsArticle>()
                .HasIndex(n => new { n.Title,n.PublishedAt,n.Source,n.ArticleLink})
                .IsUnique();
        }
        public DbSet<NewsArticle> Articles => Set<NewsArticle>();
    }
}
