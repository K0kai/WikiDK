using Microsoft.EntityFrameworkCore;
using WikiDK.Controllers;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class ArticleGroupService
    {
        private AppDbContext _appDbContext;
        private ArticleService _articleService;

        public ArticleGroupService(AppDbContext appDbContext, ArticleService articleService)
        {
            _appDbContext = appDbContext;
            _articleService = articleService;
        }

        public async Task<ArticleGroup> CreateGroup(GroupDTO groupDTO, bool locked = false)
        {
            var newGroup = new ArticleGroup()
            {
                Title = groupDTO.Title ?? throw new Exception("Title can't be null"),
                Description = groupDTO.Description ?? "",
                Locked = locked,
                DisplayOnHome = groupDTO.DisplayHome ?? false,
                DisplayOnSidebar = groupDTO.DisplaySidebar ?? false
            };
            _appDbContext.ArticleGroups.Add(newGroup);
            await _appDbContext.SaveChangesAsync();
            return newGroup;

        }
        public async Task<ArticleGroup> UpdateGroup(int groupId, GroupDTO groupDTO)
        {
            var group = _appDbContext.ArticleGroups.Find(groupId) ?? throw new Exception("No such group");
            group.Title = groupDTO.Title ?? group.Title;
            group.Description = groupDTO.Description ?? group.Description;
            group.DisplayOnHome = groupDTO.DisplayHome ?? group.DisplayOnHome;
            group.DisplayOnSidebar = groupDTO.DisplaySidebar ?? group.DisplayOnSidebar;

            await _appDbContext.SaveChangesAsync();
            return group;
        }

        public async Task<ArticleGroup?> GetGroup(int id)
        {
            return await _appDbContext.ArticleGroups.FindAsync(id);
        }

        public async Task<List<ArticleGroup>> GetGroups()
        {
            return await _appDbContext.ArticleGroups.OrderBy(g => g.Title).Include(g => g.Items).ToListAsync();
        }

        public async Task<ArticleGroupItem> GroupArticle(int articleId, int groupId, bool preventSave = false)
        {
            var article = await _articleService.GetById(articleId) ?? throw new Exception("Article doesn't exist");

            if (_appDbContext.ArticleGroupItems.Find(articleId, groupId) != null)
                throw new Exception("This article is already grouped, did you mean to reposition it?");

            var lastPosition =
    (_appDbContext.ArticleGroupItems.Where(ag => ag.ArticleGroupId == groupId)
        .Max(ah => (int?)ah.Position) ?? 0) + 1;

            var article_group_item = new ArticleGroupItem()
            {
                ArticleId = articleId,
                Position = lastPosition,
                ArticleGroupId = groupId
            };

            await _appDbContext.ArticleGroupItems.AddAsync(article_group_item);
            if (!preventSave)
                await _appDbContext.SaveChangesAsync();
            return article_group_item;
        }
        public async Task<List<ArticleGroupItem>> GroupArticleMultiple(int articleId, ICollection<int> groupIds)
        {
            var groupItems = new List<ArticleGroupItem>();
            foreach (var id in groupIds)
            {
                try
                {
                    var res = await GroupArticle(articleId, id, preventSave: true);
                    groupItems.Add(res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to group article: {articleId} and group: {id}\n{ex}");
                }
            }
            await _appDbContext.SaveChangesAsync();
            return groupItems;
        }

        public async Task<bool> UngroupArticle(int articleId, int groupId)
        {
            var highlight = await _appDbContext.ArticleGroupItems.FirstOrDefaultAsync(x => x.ArticleId == articleId && x.ArticleGroupId == groupId);

            if (highlight != null)
            {
                _appDbContext.Remove(highlight);
                return true;
            }
            await _appDbContext.SaveChangesAsync();
            return false;
        }

        public async Task<ArticleGroup?> SortGroup(int groupId)
        {
            var highlights = await _appDbContext.ArticleGroupItems.Where(x => x.ArticleGroupId == groupId).OrderBy(h => h.Position).ToListAsync();
            var increment = 1;
            foreach (var highlight in highlights)
            {
                highlight.Position = increment;
                increment++;
            }
            await _appDbContext.SaveChangesAsync();
            return await _appDbContext.ArticleGroups.FindAsync(groupId);

        }

        public async Task<ArticleGroupItem> RepositionArticle(int articleId, int groupId, int newPosition)
        {
            var articleHighlight =
                await _appDbContext.ArticleGroupItems
                .FirstOrDefaultAsync(x =>
                    x.ArticleId == articleId && x.ArticleGroupId == groupId);

            if (articleHighlight == null)
                throw new Exception("Highlight not found");

            int oldPosition = articleHighlight.Position;

            if (oldPosition == newPosition)
                return articleHighlight;

            // Moving DOWN
            if (newPosition > oldPosition)
            {
                var affected =
                    await _appDbContext.ArticleGroupItems
                    .Where(x =>
                        x.ArticleId == articleId &&
                        x.ArticleGroupId == groupId &&
                        x.Position > oldPosition &&
                        x.Position <= newPosition)
                    .ToListAsync();

                foreach (var item in affected)
                {
                    item.Position--;
                }
            }
            // Moving UP
            else
            {
                var affected =
                    await _appDbContext.ArticleGroupItems
                    .Where(x =>
                        x.ArticleId == articleId &&
                        x.ArticleGroupId == groupId &&
                        x.Position >= newPosition &&
                        x.Position < oldPosition)
                    .ToListAsync();

                foreach (var item in affected)
                {
                    item.Position++;
                }
            }

            articleHighlight.Position = newPosition;

            await _appDbContext.SaveChangesAsync();

            return articleHighlight;
        }
    }
}
