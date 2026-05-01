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
        private readonly UserService _userService;

        public ArticleController(ArticleService articleService, UserService userService)
        {
            _articleService = articleService;
            _userService = userService;
        }
        /// <summary>
        /// API Endpoint to get an article by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            var article = await _articleService.GetById(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }
        /// <summary>
        /// API Endpoint to get all articles.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _articleService.GetAll();
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
                await _articleService.Update(id, request.Title, request.Content, user.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
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
    }
    public class PublishArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
    public class UpdateArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
