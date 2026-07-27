using WikiDK.Helpers;
using WikiDK.Objects;
using WikiDK.Repositories;
using WikiDK.Services.Interfaces;

namespace WikiDK.Services
{
    public class WikiPageService (LoggerHelper<IWikiPageService> logger, AppDbContext appDbContext) : IWikiPageService
    {

        public Task<WikiPage> CreateWikiPageAsync(WikiPageCreateRequest request)
        {
            var wikiPage = new WikiPage();

            var dateNow = DateTimeOffset.UtcNow;

            wikiPage.Title = request.Title;
            wikiPage.Content = request.Content ?? string.Empty;
            wikiPage.AuthorId = request.Author!.Id;
            wikiPage.AuthorName = request.AuthorName;
            wikiPage.CreatedAt = dateNow;
            wikiPage.UpdatedAt = dateNow;
            wikiPage.Slug = SlugHelper.Slugify(request.Title);

            var slugConflictQuery = appDbContext.WikiPages.Where(wp => wp.Slug == wikiPage.Slug);

            if (slugConflictQuery.Any())
            {
                var slugConflictCount = slugConflictQuery.Count();
                wikiPage.Slug += $"-{slugConflictCount + 1}";
            }

            appDbContext.WikiPages.Add(wikiPage);

            appDbContext.SaveChanges();

            return Task.FromResult(wikiPage);
        }

        public bool DeleteWikiPage(string slugOrId)
        {
            var IsId = int.TryParse(slugOrId, out var id);

            var wikiPage = IsId ? appDbContext.WikiPages.Find(id) : appDbContext.WikiPages.FirstOrDefault(wp => wp.Slug == slugOrId);
            logger.LogInformation("Deleting wiki page of Id: {id} && Name: {title} ", id, wikiPage?.Title);
            if (wikiPage == null)
            {
                logger.LogInformation("Wiki page doesn't exist, returning false");
                return false;
            }
            appDbContext.WikiPages.Remove(wikiPage);
            appDbContext.SaveChanges();
            logger.LogInformation("Deleted {id}", id);
            return true;
        }      

        public Task<List<WikiPage>> GetAllWikiPagesAsync()
        {
            var wikiPages = appDbContext.WikiPages.ToList();
            return Task.FromResult(wikiPages);
        }

        public Task<WikiPage?> GetWikiPageByIdAsync(int id)
        {
            var wikiPage = appDbContext.WikiPages.Find(id);
            return Task.FromResult(wikiPage);
        }

        public Task<WikiPage?> GetWikiPageBySlug(string slug)
        {
            var wikiPage = appDbContext.WikiPages.FirstOrDefault(wp => wp.Slug == slug);
            return Task.FromResult(wikiPage);
        }

        public Task<WikiPage?> GetWikiPageByTitleAsync(string title)
        {
            var wikiPage = appDbContext.WikiPages.FirstOrDefault(wp => wp.Title == title);
            return Task.FromResult(wikiPage);
        }

        public bool UpdateWikiPage(int id, WikiPageUpdateRequest request)
        {
            var wikiPage = appDbContext.WikiPages.Find(id);
            if (wikiPage == null)
            {
                return false;
            }
            UpdateWiki(wikiPage, request);
            appDbContext.SaveChanges();
            return true;
        }
        public bool UpdateWikiPage(string slug, WikiPageUpdateRequest request)
        {
            var wikiPage = appDbContext.WikiPages.FirstOrDefault(wp => wp.Slug == slug);
            if (wikiPage == null)
            {
                return false;
            }
            UpdateWiki(wikiPage, request);
            appDbContext.SaveChanges();
            return true;
        }
        private static void UpdateWiki(WikiPage wikiPage, WikiPageUpdateRequest request)
        {
            wikiPage.Title = request.Title;
            wikiPage.Content = request.Content ?? string.Empty;
            wikiPage.UpdatedAt = DateTimeOffset.UtcNow;
            wikiPage.EditorId = request.Editor?.Id ?? throw new Exception("Editor cannot be null");
        }
    }
}
