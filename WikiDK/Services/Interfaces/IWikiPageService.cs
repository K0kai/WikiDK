using WikiDK.Objects;

namespace WikiDK.Services.Interfaces
{
    public interface IWikiPageService
    {
        public Task<WikiPage?> GetWikiPageByIdAsync(int id);
        public Task<WikiPage?> GetWikiPageByTitleAsync(string title);
        public Task<WikiPage?> GetWikiPageBySlug(string slug);
        public Task<List<WikiPage>> GetAllWikiPagesAsync();
        public Task<WikiPage> CreateWikiPageAsync(WikiPageCreateRequest request);
        public bool DeleteWikiPageAsync(int id);
        public bool UpdateWikiPageAsync(int id, WikiPageUpdateRequest request);
    }
}
