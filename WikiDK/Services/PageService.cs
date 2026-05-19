using Microsoft.EntityFrameworkCore;
using WikiDK.Controllers;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class PageService
    {
        private AppDbContext appDbContext;
        private int SectionLimit = 10;
        public PageService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PageSection> CreateSection(PageSectionCreateRequest request)
        {
            var sectionCount = appDbContext.PageSections.Count();
            if (sectionCount >= SectionLimit)
                throw new Exception("Section limit reached");

            var section = new PageSection()
            {
                Title = request.Title,
                Content = request.Content,
                Order = request.Order,
                IsVisible = request.isVisible
            };
            await appDbContext.PageSections.AddAsync(section);
            await appDbContext.SaveChangesAsync();
            return section;
        }
        public async Task<PageSection?> GetSection(int id)
        {
            return await appDbContext.PageSections.FindAsync(id);
        }
        public int GetSectionLimit()
        {
            return SectionLimit;
        }
        public async Task<List<PageSection>> GetSections()
        {
            return await appDbContext.PageSections.ToListAsync();
        }
    }
}
