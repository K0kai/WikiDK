using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;

namespace WikiDK.Repositories
{
    /// <summary>
    /// A class to serve as the database context for the application, inheriting from DbContext. It defines DbSets for User and Article entities and configures the model using OnModelCreating.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Define your DbSets here
        // public DbSet<User> Users { get; set; }
        // public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArticleGroup> ArticleGroups { get; set; }
        public DbSet<ArticleGroupItem> ArticleGroupItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
            modelBuilder.Entity<ArticleCategory>()
       .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

            modelBuilder.Entity<History>()
    .HasOne(h => h.Article)
    .WithMany()
    .HasForeignKey(h => h.ArticleId)
    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
