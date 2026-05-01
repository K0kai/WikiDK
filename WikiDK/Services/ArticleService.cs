using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class ArticleService
    {
        private readonly AppDbContext _dbContext;
        private readonly HistoryService _historySvc;

        public ArticleService(AppDbContext context, HistoryService historySvc)
        {
            _dbContext = context;
            _historySvc = historySvc;
        }
        /// <summary>
        /// Creates and publishes a new article with the given title, content, and author ID. The article is added to the database and saved.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Attempts to return an article from the database by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Article?> GetById(int id)
        {
            return await _dbContext.Articles.FindAsync(id);
        }
        /// <summary>
        /// Attempts to return every article from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article>> GetAll()
        {
            return await _dbContext.Articles.ToListAsync();
        }
        /// <summary>
        /// Updates an existing article in the database with new title, content and author ID. If the article with the given ID isn't found an exception is thrown.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Update(int id, string title, string content, int authorId)
        {
            var article = await GetById(id) ?? throw new Exception("Article not found");            
            await Update(article, title, content, authorId);
        }
        /// <summary>
        /// Updates an existing article in the database with new title, content and author ID. If the article with the given ID isn't found an exception is thrown.
        /// </summary>
        /// <param name="article"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Update(Article article, string title, string content, int authorId)
        {          
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Title cannot be empty");

            var history = new History()
            {
                ArticleId = article.Id,
                EditorId = authorId,
                PreviousTitle = article.Title,
                PreviousContent = article.Content,
                EditDate = DateTime.Now
            };

            article.Title = title;
            article.Content = content;
            article.AuthorId = authorId;
            article.Updated = DateTime.Now;            
            _dbContext.Update(article);
            await _dbContext.SaveChangesAsync();
            await _historySvc.CreateHistory(history);
        }
        /// <summary>
        /// Deletes an article from the database from its id. If the article with the given ID isn't found an exception is thrown.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Delete(int id)
        {
            var article = await GetById(id) ?? throw new Exception("Article not found");
            await Delete(article);
        }
        /// <summary>
        /// Deletes an article from the database. If the article with the given ID isn't found an exception is thrown.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Delete(Article article)
        {
            _dbContext.Articles.Remove(article);
            await _dbContext.SaveChangesAsync();
        }
    }
}
