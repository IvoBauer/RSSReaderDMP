using Microsoft.EntityFrameworkCore;
using RSSReader.Models;

namespace RSSReader.Data
{
    public class ApplicationDbContext : DbContext
    {        
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
