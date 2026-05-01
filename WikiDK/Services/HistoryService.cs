using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class HistoryService
    {
        public HistoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private AppDbContext _dbContext;

        /// <summary>
        /// Adds a new history record to the database and saves changes asynchronously.
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task CreateHistory(History history)
        {
            _dbContext.Histories.Add(history);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Returns a history record by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<History?> GetHistoryById(int id)
        {
            var history = await _dbContext.Histories.FindAsync(id);
            return history;
        }
        /// <summary>
        /// Gets every history record associated with an article by the article's ID
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<History>> GetAllHistoriesAssociatedWithArticle(int articleId)
        {
            var histories = await _dbContext.Histories.Where(h => h.ArticleId == articleId).ToListAsync();
            return histories;
        }
        /// <summary>
        /// Gets every history record associated with an editor by their ID
        /// </summary>
        /// <param name="editorId"></param>
        /// <returns></returns>
        public async Task<List<History>> GetAllHistoriesAssociatedWithEditor(int editorId)
        {
            var histories = await _dbContext.Histories.Where(h => h.EditorId == editorId).ToListAsync();
            return histories;
        }
    }
}
