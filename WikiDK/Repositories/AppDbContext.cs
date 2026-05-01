using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;

namespace WikiDK.Repositories
{
    /// <summary>
    /// A class to serve as the database context for the application, inheriting from DbContext. It defines DbSets for User and Article entities and configures the model using OnModelCreating.
    /// </summary>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}
