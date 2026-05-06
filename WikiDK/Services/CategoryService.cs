using WikiDK.Controllers;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class CategoryService
    {
        private AppDbContext _appDbContext;
        public CategoryService(ArticleService articleService, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task CreateCategory(CategoryCreateRequest CCR)
        {
            var newCategory = new Category()
            {
                Name = CCR.Name,
                Description = CCR.Description,
                Slug = CCR.Slug
            };
            await CreateCategory(newCategory);
        }
        public async Task CreateCategory(Category category)
        {
            _appDbContext.Categories.Add(category);
            await _appDbContext.SaveChangesAsync();
        }
        public void UpdateCategory(Category category)
        {
            _appDbContext.Categories.Update(category);
            _appDbContext.SaveChanges();
        }
        public async Task DeleteCategory(Category category)
        {
            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<Category?> GetById(int id)
        {
            return await _appDbContext.Categories.FindAsync(id);
        }
        public async Task<List<Category>> GetAll()
        {
            return [.. _appDbContext.Categories];
        }
    }
}
