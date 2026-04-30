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
        [Authorize]
        [HttpPost("publish")]
        public async Task<IActionResult> PublishArticle([FromBody] PublishArticleRequest request)
        {
            var article = await _articleService.Publish(request.Title, request.Content, request.AuthorId);
            return Ok(article);
        }
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] UpdateArticleRequest request)
        {
            try
            {
                var user = await _userService.GetByName(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User needs to be logged in")) ?? throw new Exception("User not found");
                await _articleService.Update(id, request.Title, request.Content, user.Id);
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
        public int AuthorId { get; set; }
    }
    public class UpdateArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
