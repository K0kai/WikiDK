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
                IsVisible = request.IsVisible
            };
            await appDbContext.PageSections.AddAsync(section);
            await appDbContext.SaveChangesAsync();
            return section;
        }
        public async Task<PageSection?> UpdateSection(int id, PageSectionCreateRequest request)
        {
            var section = await appDbContext.PageSections.FindAsync(id);

            if (section == null)
                return null;

            section.Title = request.Title;
            section.Content = request.Content;
            section.Order = request.Order;
            section.IsVisible = request.IsVisible;

            await appDbContext.SaveChangesAsync();
            return section;
        }
        public async Task<List<PageSection>> ReorderSessions(List<ReorderRequest> reorders)
        {
            var ids = reorders.Select(r => r.Id).Where(i => i > 0);
            var sections = await appDbContext.PageSections.Where(s => ids.Contains(s.Id)).ToListAsync();

            foreach (var section in sections)
            {
                var reorderRequest = reorders.FirstOrDefault(r => r.Id == section.Id);
                if (reorderRequest == null)
                    continue;

                var treatedOrder = Math.Max(1, Math.Min(10, reorderRequest.Order));
                section.Order = treatedOrder;
            }
            await appDbContext.SaveChangesAsync();
            return sections;
        }
        public async Task<PageSection?> RegenSlug(int id)
        {
            var section = await appDbContext.PageSections.FindAsync(id);
            if (section == null)
                return null;

            section.GenerateSlug();
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
            return await appDbContext.PageSections.OrderBy(pg => pg.Order).ToListAsync();
        }
    }
}
