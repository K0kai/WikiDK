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
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArticleGroup> ArticleGroups { get; set; }
        public DbSet<ArticleGroupItem> ArticleGroupItems { get; set; }
        public DbSet<ArticleSubmission> ArticleSubmissions { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<PageSection> PageSections { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Policy_Permission> PolicyPermissions { get; set; }
        public DbSet<Role_Policy> RolePolicies { get; set; }
        public DbSet<WikiPage> WikiPages { get; set; }

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var newArticles = ChangeTracker
                .Entries<Article>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .ToList();

            var newSections = ChangeTracker
                .Entries<PageSection>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var article in newArticles)
            {
                if (string.IsNullOrWhiteSpace(article.Slug))
                {
                    article.GenerateSlug();
                }
            }

            foreach (var section in newSections)
            {
                if (string.IsNullOrWhiteSpace(section.Slug))
                {
                    section.GenerateSlug();
                }
            }

            if (newArticles.Count != 0)
            {
                await base.SaveChangesAsync(cancellationToken);
            }
            if (newSections.Count != 0)
            {
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
