using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [Route("articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _articleService;
        private readonly ArticleCategoryService _articleCategoryService;
        private readonly UserService _userService;

        public ArticleController(ArticleService articleService, UserService userService, ArticleCategoryService articleCategoryService)
        {
            _articleService = articleService;
            _userService = userService;
            _articleCategoryService = articleCategoryService;

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
        [HttpGet("get")]
        public async Task<IActionResult> GetAllArticles([FromBody] int page = 1, int pageSize = 50)
        {
            const int MaxPageSize = 50;
            const int MinPageSize = 10;
            pageSize = Math.Min(pageSize, MaxPageSize);
            if (pageSize <= 0)
                pageSize = MinPageSize;
            try
            {
                var articles = await _articleService.GetPaginated(page, pageSize);
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
            var article = await _articleService.Publish(request.Title, request.Content, id);
            return Ok(article);
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
                await _articleService.Update(id, request.Title, request.Content, user.Id, request.ThumbnailLink);
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
        [HttpDelete("{articleId}/category/{categoryId}/delete")]
        [Authorize(Roles = "Admin,Editor,Owner")]
        public async Task <IActionResult> UncategorizeArticle(int articleId, int categoryId)
        {
            try
            {
                await _articleCategoryService.RemoveArticleFromCategory(articleId, categoryId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
    public class PublishArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ThumbnailLink { get; set; } = string.Empty;
    }
    public class UpdateArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ThumbnailLink { get; set; } = string.Empty;
    }
}
