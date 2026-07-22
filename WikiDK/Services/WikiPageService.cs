using WikiDK.Helpers;
using WikiDK.Objects;
using WikiDK.Repositories;
using WikiDK.Services.Interfaces;

namespace WikiDK.Services
{
    public class WikiPageService : IWikiPageService
    {
        private readonly AppDbContext _appDbContext;

        public WikiPageService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<WikiPage> CreateWikiPageAsync(WikiPageCreateRequest request)
        {
            var wikiPage = new WikiPage();

            var dateNow = DateTimeOffset.UtcNow;

            wikiPage.Title = request.Title;
            wikiPage.Content = request.Content ?? string.Empty;
            wikiPage.AuthorId = request.AuthorId;
            wikiPage.AuthorName = request.AuthorName;
            wikiPage.EditorId = request.AuthorId;
            wikiPage.CreatedAt = dateNow;
            wikiPage.UpdatedAt = dateNow;
            wikiPage.Slug = SlugHelper.Slugify(request.Title);

            var slugConflictQuery = _appDbContext.WikiPages.Where(wp => wp.Slug == wikiPage.Slug);

            if (slugConflictQuery.Any())
            {
                var slugConflictCount = slugConflictQuery.Count();
                wikiPage.Slug += $"-{slugConflictCount + 1}";
            }

            _appDbContext.WikiPages.Add(wikiPage);

            _appDbContext.SaveChanges();

            return Task.FromResult(wikiPage);
        }

        public bool DeleteWikiPage(int id)
        {
            var wikiPage = _appDbContext.WikiPages.Find(id);
            if (wikiPage == null)
            {
                return false;
            }
            _appDbContext.WikiPages.Remove(wikiPage);
            _appDbContext.SaveChanges();
            return true;
        }

        public Task<List<WikiPage>> GetAllWikiPagesAsync()
        {
            var wikiPages = _appDbContext.WikiPages.ToList();
            return Task.FromResult(wikiPages);
        }

        public Task<WikiPage?> GetWikiPageByIdAsync(int id)
        {
            var wikiPage = _appDbContext.WikiPages.Find(id);
            return Task.FromResult(wikiPage);
        }

        public Task<WikiPage?> GetWikiPageBySlug(string slug)
        {
            var wikiPage = _appDbContext.WikiPages.FirstOrDefault(wp => wp.Slug == slug);
            return Task.FromResult(wikiPage);
        }

        public Task<WikiPage?> GetWikiPageByTitleAsync(string title)
        {
            var wikiPage = _appDbContext.WikiPages.FirstOrDefault(wp => wp.Title == title);
            return Task.FromResult(wikiPage);
        }

        public bool UpdateWikiPage(int id, WikiPageUpdateRequest request)
        {
            var wikiPage = _appDbContext.WikiPages.Find(id);
            if (wikiPage == null)
            {
                return false;
            }

            wikiPage.Title = request.Title;
            wikiPage.Content = request.Content ?? string.Empty;
            wikiPage.UpdatedAt = DateTimeOffset.UtcNow;
            wikiPage.EditorId = request.EditorId;

            _appDbContext.SaveChanges();
            return true;
        }
        public bool UpdateWikiPage(string slug, WikiPageUpdateRequest request)
        {
            var wikiPage = _appDbContext.WikiPages.FirstOrDefault(wp => wp.Slug == slug);
            if (wikiPage == null)
            {
                return false;
            }
            wikiPage.Title = request.Title;
            wikiPage.Content = request.Content ?? string.Empty;
            wikiPage.UpdatedAt = DateTimeOffset.UtcNow;
            wikiPage.EditorId = request.EditorId;
            _appDbContext.SaveChanges();
            return true;
        }
    }
}
