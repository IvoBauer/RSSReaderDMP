using Microsoft.EntityFrameworkCore;
using RSSReader.Models;

namespace RSSReader.Data
{
    public class ApplicationDbContext : DbContext
    {        
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<FeedCategory> FeedCategories { get; set; }
        public DbSet<FeedCategoryRecord> FeedCategoryRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
