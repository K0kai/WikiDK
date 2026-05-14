using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json.Serialization;
using WikiDK.Objects;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _articleService;
        private readonly ArticleGroupService _articleGroupService;
        private readonly ArticleCategoryService _articleCategoryService;
        private readonly UserService _userService;

        public ArticleController(ArticleService articleService, UserService userService, ArticleCategoryService articleCategoryService, ArticleGroupService articleGroupService)
        {
            _articleService = articleService;
            _userService = userService;
            _articleCategoryService = articleCategoryService;
            _articleGroupService = articleGroupService;

        }
        /// <summary>
        /// API Endpoint to get an article by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            try
            {

                var article = await _articleService.GetById(id);
                if (article == null)
                {
                    return NotFound();
                }
                return Ok(article);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred in articles/get{id} endpoint");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// API endpoint to get all articles. The limit of articles returned is defined by page size, with the max being 50.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> GetAllArticles([FromBody] GetArticlesParams getParams)
        {
            try
            {
                Debug.WriteLine($"filters:{string.Join(',',getParams.CategoryFilters)}");
                var articles = await _articleCategoryService.GetPaginatedAndFiltered(getParams);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// API Endpoint to get a limited number of recent articles.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("get/recent/limit/{limit}")]
        public async Task<IActionResult> GetRecentArticles(int limit)
        {
            var articles = await _articleService.GetRecent(limit);
            return Ok(articles);
        }
        /// <summary>
        /// API Endpoint to publish an article.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Editor,Owner")]
        [HttpPost("publish")]
        public async Task<IActionResult> PublishArticle([FromBody] PublishArticleRequest request)
        {
            var validId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id);
            if (!validId)
                return BadRequest("Invalid Id");
            request.AuthorId = id;
            var article = await _articleService.Publish(request);
            var groupItems = new List<ArticleGroupItem>();
            if (request.Groups != null && request.Groups.Count > 0)
            {
               groupItems = await _articleGroupService.GroupArticleMultiple(article.Id, request.Groups);
            }
            return CreatedAtAction("Article created successfully", new { article, articleGroupItems = groupItems});
        }
        /// <summary>
        /// API Endpoint to update an existing article.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Editor,Owner")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] UpdateArticleRequest request)
        {
            try
            {
                var validId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
                var user = await _userService.GetById(userId) ?? throw new Exception("User not found");
                await _articleService.Update(id, user.Id, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// API Endpoint to delete an article.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                await _articleService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{articleId}/category/{categoryId}")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task<IActionResult> CategorizeArticle(int articleId, int categoryId)
        {
            try
            {
                await _articleCategoryService.CategorizeArticle(articleId, categoryId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{articleId}/category")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task<IActionResult> CategorizeArticle(int articleId, [FromBody] int[] categoryIds)
        {
            try
            {
                Console.WriteLine($"Categories: {string.Join(',', categoryIds)}");
                await _articleCategoryService.CategorizeArticle(articleId, categoryIds);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{articleId}/category/{categoryId}/delete")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task<IActionResult> UncategorizeArticle(int articleId, int categoryId)
        {
            try
            {
                await _articleCategoryService.RemoveArticleFromCategory(articleId, categoryId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("group/{articleId}:{groupId}")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task<IActionResult> GroupArticle(int articleId, int groupId)
        {
            try
            {
                var highlightObject = await _articleGroupService.GroupArticle(articleId, groupId);
                return Ok(highlightObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ungroup/{articleId}:{groupId}")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task<IActionResult> UngroupArticle(int articleId, int groupId)
        {
            try
            {
                var groupObject = await _articleGroupService.UngroupArticle(articleId, groupId);

                if (!groupObject) throw new Exception($"Failed to remove article {articleId} highlight");

                await _articleGroupService.SortGroup(groupId);
                return Ok(groupObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var groups = await _articleGroupService.GetGroups();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetGroup(int groupId)
        {
            try
            {
                var group = await _articleGroupService.GetGroup(groupId) ?? throw new Exception("Group does not exist");
                return Ok(group);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("group")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO groupDTO)
        {
            try
            {
                var group = await _articleGroupService.CreateGroup(groupDTO);
                return Ok(group);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("group/update/{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] GroupDTO groupDTO)
        {
            try
            {
                var updatedGroup = await _articleGroupService.UpdateGroup(groupId, groupDTO);
                return Ok(updatedGroup);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }
    public class GroupDTO
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? DisplayHome { get; set; } = false;
        public bool? DisplaySidebar { get; set; } = false;
    }
    public class PublishArticleRequest
    {
        [JsonIgnore]
        public int? AuthorId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ThumbnailLink { get; set; } = string.Empty;
        public List<int>? Groups { get; set; } = [];
        public List<int>? Categories { get; set; } = [];
    }
    public class UpdateArticleRequest
    {
        public string? Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public string? ThumbnailLink { get; set; } = string.Empty;
        public List<int>? Categories { get; set; } = [];
        public List<int>? Groups { get; set; } = [];
    }
    public class GetArticlesParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateSortType DateSortType { get; set; } = DateSortType.UpdatedNewest;
        public List<int> CategoryFilters { get; set; } = [];

        public GetArticlesParams()
        {
            if (PageSize > 50)
            {
                PageSize = 50;
            }
            if (PageSize <= 0)
            {
                PageSize = 10;
            }
        }

    }
    public enum DateSortType
    {
        None,
        UpdatedNewest,
        UpdatedOldest,
        CreatedOldest,
        CreatedNewest
    }
    public class CategoryFilter
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
    }
}
