using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;

namespace WikiDK.Repositories
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Define your DbSets here
        // public DbSet<User> Users { get; set; }
        // public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
