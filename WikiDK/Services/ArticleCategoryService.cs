using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CategorizeArticle(int articleId, int categoryId)
        {
            var category = _categoryService.GetById(categoryId);
            var article = _articleService.GetById(articleId);

            if (category == null)
                throw new Exception("Category is missing");
            if (article == null)
                throw new Exception("Article is missing");

            var articleCategory = new ArticleCategory() { ArticleId = articleId, CategoryId = categoryId };

            _appDbContext.Article_Category.Add(articleCategory);
            return true;
        }

        public async Task<bool> RemoveArticleFromCategory(int articleId, int categoryId)
        {
            var articleCat = _appDbContext.Article_Category.Find(articleId, categoryId);
            if (articleCat != null)
            {
                _appDbContext.Article_Category.Remove(articleCat);
                _appDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<Article>> GetArticlesByCategory(int categoryId)
        {
            List<ArticleCategory> artCat = [.. _appDbContext.Article_Category.Include(a => a.Article).Where(x => x.CategoryId == categoryId)];
            List<Article> articles = [.. artCat.Select(a => a.Article)];
            return articles;
        } 
    }
}
