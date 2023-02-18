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
           
        }
        public DbSet<ExternalContentLink> ExternalContentLinks => Set<ExternalContentLink>();
    }
}
