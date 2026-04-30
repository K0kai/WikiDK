using Microsoft.AspNetCore.Mvc;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [Route("articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }
    }
}
