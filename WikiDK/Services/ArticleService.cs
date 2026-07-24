using Microsoft.EntityFrameworkCore;
using WikiDK.Controllers;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class ArticleService
    {
        private readonly AppDbContext _dbContext;
        private readonly HistoryService _historySvc;
        private readonly CloudinaryService _cloudinaryService;

        private readonly int maxSubmissionPageSize = 10;

        public ArticleService(AppDbContext context, HistoryService historySvc, CloudinaryService cloudinaryService)
        {
            _dbContext = context;
            _historySvc = historySvc;
            _cloudinaryService = cloudinaryService;
        }
        /// <summary>
        /// Creates and publishes a new article with the given title, content, and author ID. The article is added to the database and saved.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Article> Publish(PublishArticleRequest request)
        {
            // Create a new article
            var utcNowDate = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(request.ThumbnailLink))
                request.ThumbnailLink = null;
            Article article = new()
            {
                Title = request.Title,
                Content = request.Content,
                Created = utcNowDate,
                Updated = utcNowDate,
                AuthorId = request.AuthorId ?? throw new Exception("Author id cannot be null"),
                LastEditorId = request.AuthorId ?? throw new Exception("Author id cannot be null"),
                ThumbnailLink = request.ThumbnailLink,
                Categories = request.Categories ?? []
            };

            if (article.AuthorId == 0)
                throw new Exception("Invalid author Id");
            // Add article to the database and save changes
            _dbContext.Articles.Add(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }
        public async Task<List<Article>?> GetById(int[] ids)
        {
            return await _dbContext.Articles.Where(a => ids.Contains(a.Id)).ToListAsync();
        }
        /// <summary>
        /// Attempts to return an article from the database by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Article?> GetById(int id)
        {
            return await _dbContext.Articles.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Article?> GetBySlug(string slug)
        {
            return await _dbContext.Articles.FirstOrDefaultAsync(a => a.Slug == slug);
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
        /// Attempts to return a specific amount of articles, defined by page and page size.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<Article>> GetPaginated(int page, int pageSize)
        {
            var query = _dbContext.Articles.OrderByDescending(a => a.Updated);
            var totalCount = await query.CountAsync();

            var articles = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return articles;
        }
        /// <summary>
        /// Attempts to return the most recent articles from the database, ordered by their last updated date in descending order, limited by the specified number of articles.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<Article>> GetRecent(int limit)
        {
            return await _dbContext.Articles.OrderByDescending(a => a.Updated).Take(limit).ToListAsync();
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
        public async Task<Article> Update(int id, int authorId, UpdateArticleRequest UAR)
        {
            var article = await GetById(id) ?? throw new Exception("Article not found");
            return await Update(article, authorId, UAR);
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
        public async Task<Article> Update(Article article, int authorId, UpdateArticleRequest UAR)
        {
            if (string.IsNullOrWhiteSpace(UAR.Title))
                throw new Exception("Title cannot be empty");
            var utcNowDate = DateTime.UtcNow;
            var history = new History()
            {
                ArticleId = article.Id,
                EditorId = authorId,
                PreviousTitle = article.Title,
                PreviousContent = article.Content,
                PreviousThumbnailLink = string.IsNullOrWhiteSpace(article.ThumbnailLink) ? null : article.ThumbnailLink,
                EditDate = utcNowDate
            };
            if (string.IsNullOrWhiteSpace(UAR.ThumbnailLink))
                UAR.ThumbnailLink = null;
            article.Title = string.IsNullOrWhiteSpace(UAR.Title) ? article.Title : UAR.Title;
            article.Content = UAR.Content ?? article.Content;
            article.AuthorId = authorId;
            article.Updated = utcNowDate;
            article.ThumbnailLink = UAR.ThumbnailLink ?? article.ThumbnailLink;
            article.Categories = UAR.Categories ?? article.Categories;



            if (UAR.Groups != null)
            {
                foreach (var id in UAR.Groups)
                {
                    var groupItem = _dbContext.ArticleGroupItems.FirstOrDefault(x => x.ArticleId == article.Id && x.ArticleGroupId == id);
                    if (groupItem != null)
                        continue;

                    var lastPosition = _dbContext.ArticleGroupItems.Any() ? _dbContext.ArticleGroupItems.Max(x => x.Position) : 0;
                    var newGroupItem = new ArticleGroupItem() { ArticleGroupId = id, ArticleId = article.Id, Position = ++lastPosition };
                    _dbContext.ArticleGroupItems.Add(newGroupItem);

                }
                var excludedGroups = _dbContext.ArticleGroupItems.Where(x => x.ArticleId == article.Id && !UAR.Groups.Contains(x.ArticleGroupId));
                _dbContext.RemoveRange(excludedGroups);
            }
            await _dbContext.SaveChangesAsync();
            await _historySvc.CreateHistory(history);
            return article;
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
        /// <summary>
        /// Reverts changes done to an article by the previous content specified in the history ID. If the history with the given ID isn't found an exception is thrown.
        /// </summary>
        /// <param name="historyId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Article> RevertChanges(int historyId)
        {
            var history = await _historySvc.GetHistoryById(historyId) ?? throw new Exception("History not found");
            return await RevertChanges(history);
        }
        /// <summary>
        /// Reverts changes done to an article by the previous content specified. If the history or article isn't found an exception is thrown.
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Article> RevertChanges(History history)
        {
            var article = await GetById(history.ArticleId) ?? throw new Exception("Article not found");

            article.Title = history.PreviousTitle;
            article.Content = history.PreviousContent;
            article.Updated = DateTime.UtcNow;
            _dbContext.Update(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }
        public async Task<bool> SubmitRequest(ArticleSubmission articleSubmission, IFormFile? thumbnailFile = null)
        {
            switch (articleSubmission.Type)
            {
                case "create":
                    if (articleSubmission.ArticleId != null)
                        throw new Exception("Invalid submission type, cannot create when an article is already assigned");
                    break;
                case "update":
                    if (articleSubmission.ArticleId == null)
                        throw new Exception("Invalid submission type, cannot update when an article is not assigned");
                    _ = await GetById((int)articleSubmission.ArticleId) ?? throw new Exception("This is not a valid submission, article to be updated doesn't exist");
                    break;
                default:
                    throw new Exception($"Unhandled case: {articleSubmission.Type}");
            }
            if (thumbnailFile != null)
            {
                var thumbnailUrl = await _cloudinaryService.UploadImage(thumbnailFile);
                articleSubmission.ArticleThumbnail = thumbnailUrl;
            }
            await _dbContext.ArticleSubmissions.AddAsync(articleSubmission);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<Article?> ProcessSubmission(int submissionId, User reviewer)
        {
            var submission = await _dbContext.ArticleSubmissions.FindAsync(submissionId) ?? throw new Exception("Submission does not exist");

            Article article;

            if (submission.Status != "pending")
                return null;

            switch (submission.Type)
            {
                case "create":
                    var newArticle = new PublishArticleRequest()
                    {
                        Title = submission.Title,
                        Description = submission.Description,
                        Content = submission.Content ?? "",
                        AuthorId = submission.SubmitterId ?? -1,
                        ThumbnailLink = submission.ArticleThumbnail,
                        Groups = submission.Groups,
                        Categories = submission.Categories,
                    };
                    article = await Publish(newArticle);
                    break;
                case "update":
                    var updateArticle = new UpdateArticleRequest()
                    {
                        Title = submission.Title,
                        Content = submission.Content ?? "",
                        Description = submission.Description,
                        ThumbnailLink = submission.ArticleThumbnail,
                        Groups = submission.Groups,
                        Categories = submission.Categories
                    };
                    article = await Update(submission.ArticleId ?? -1, submission.SubmitterId ?? -1, updateArticle);
                    break;
                default:
                    throw new Exception($"Unhandled case: {submission.Type}");
            }

            submission.ReviewerId = reviewer.Id;
            submission.Status = "approved";

            await _dbContext.SaveChangesAsync();
            return article;
        }
        public async Task<bool> RejectSubmission(int submissionId, User reviewer)
        {
            var sub = await _dbContext.ArticleSubmissions.FindAsync(submissionId);

            if (sub == null)
                return false;

            if (sub.Status != "pending")
                return false;

            sub.ReviewerId = reviewer.Id;
            sub.Status = "rejected";

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<ArticleSubmission?> GetArticleSubmission(int id)
        {
            return await _dbContext.ArticleSubmissions.FindAsync(id);
        }
        public async Task<int> GetTotalSubmissionPages(string status, string? type = "any")
        {
            var maxPages = (int)Math.Max(1, Math.Ceiling((double)(await GetSubmissionsCount(status, type) / maxSubmissionPageSize)));
            return maxPages;
        }
        public async Task<List<ArticleSubmission>> GetPaginatedArticleSubmissions(int page, string status, string? type = "any")
        {
            var query = _dbContext.ArticleSubmissions.Where(a => a.Status == status && (type == "any" || a.Type == type)).OrderBy(a => a.Id).Skip((page - 1) * maxSubmissionPageSize).Take(maxSubmissionPageSize);
            return await query.ToListAsync();
        }
        public async Task<int> GetSubmissionsCount(string status, string? type = "any")
        {
            return await _dbContext.ArticleSubmissions.Where(a => a.Status == status && (type == "any" || a.Type == type)).CountAsync();
        }
    }
}
