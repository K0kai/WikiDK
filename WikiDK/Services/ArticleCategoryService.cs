using Microsoft.EntityFrameworkCore;
using WikiDK.Controllers;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class ArticleCategoryService
    {
        private ArticleService _articleService;
        private CategoryService _categoryService;
        private AppDbContext _appDbContext;
        public ArticleCategoryService(ArticleService articleService, CategoryService categoryService, AppDbContext appDbContext)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _appDbContext = appDbContext;
        }

        public async Task<bool> CategorizeArticle(int articleId, int[] categoryIds)
        {
            var article = await _articleService.GetById(articleId);
            article?.Categories.Clear();
            foreach (var cat in categoryIds)
            {
                await CategorizeArticle(articleId, cat);
            }
            return true;
        }

        public async Task<bool> CategorizeArticle(int articleId, int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            var article = await _articleService.GetById(articleId);

            if (category == null)
            {
                article?.Categories.Remove(categoryId);
                throw new Exception("Category is missing");
            }
            if (article == null)
                throw new Exception("Article is missing");

            if (!article.Categories.Contains(categoryId))
                article.Categories.Add(categoryId);

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveArticleFromCategory(int articleId, int categoryId)
        {
            var article = _appDbContext.Articles.Find(articleId);
            article?.Categories.Remove(categoryId);
            await _appDbContext.SaveChangesAsync();
            return false;
        }

        public async Task<List<Article>> GetArticlesByCategory(int categoryId)
        {
            return await _appDbContext.Articles.Where(x => x.Categories.Contains(categoryId)).ToListAsync();
        }

        public async Task<List<Article>> GetPaginatedAndFiltered(GetArticlesParams getArticlesParams)
        {
            var filteredCategories = getArticlesParams.CategoryFilters;

            IQueryable<Article> query = _appDbContext.Articles;

            if (filteredCategories.Any())
            {
                query = query.Where(a =>
                    _appDbContext.Articles
                        .Any(ar =>
                            ar.Id == a.Id &&
                            filteredCategories.All(fc => a.Categories.Contains(fc))
                        )
                );
            }

            query = getArticlesParams.DateSortType switch
            {
                DateSortType.UpdatedNewest => query.OrderByDescending(a => a.Updated),
                DateSortType.UpdatedOldest => query.OrderBy(a => a.Updated),
                DateSortType.CreatedOldest => query.OrderBy(a => a.Created),
                DateSortType.CreatedNewest => query.OrderByDescending(a => a.Created),
                _ => query.OrderBy(a => a.Id)
            };

            return await query.Skip((getArticlesParams.Page - 1) * getArticlesParams.PageSize)
                .Take(getArticlesParams.PageSize)
                .ToListAsync();
        }
    }
}
