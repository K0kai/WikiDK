using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class ArticleService
    {
        private readonly AppDbContext _dbContext;

        public ArticleService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Article> Publish(string title, string content, int authorId)
        {
            // Create a new article
            Article article = new()
            {
                Title = title,
                Content = content,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                AuthorId = authorId
            };
            // Add article to the database and save changes
            _dbContext.Articles.Add(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }

        public async Task<Article?> GetById(int id)
        {
            return await _dbContext.Articles.FindAsync(id);
        }

        public async Task Update(int id, string title, string content, int authorId)
        {
            var article = await GetById(id) ?? throw new Exception("Article not found");
            await Update(article, title, content, authorId);
        }

        public async Task Update(Article article, string title, string content, int authorId)
        {          
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Title cannot be empty");
            article.Title = title;
            article.Content = content;
            article.AuthorId = authorId;
            article.Updated = DateTime.Now;

            _dbContext.Update(article);
            await _dbContext.SaveChangesAsync();
        }
    }
}
