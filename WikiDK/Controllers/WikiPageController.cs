using Microsoft.AspNetCore.Mvc;
using WikiDK.Objects;
using WikiDK.Services.Interfaces;

namespace WikiDK.Controllers
{
    [Route("api/wiki")]
    [ApiController]
    public class WikiPageController : ControllerBase
    {
        private readonly IWikiPageService _wikiPageService;

        public WikiPageController(IWikiPageService wikiPageService)
        {
            _wikiPageService = wikiPageService;
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            try
            {
                var page = await _wikiPageService.GetWikiPageBySlug(slug);
                if (page == null)
                {
                    return NotFound();
                }
                return Ok(page);
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] WikiPageCreateRequest request)
        {
            try
            {
                var page = await _wikiPageService.CreateWikiPageAsync(request);
                return CreatedAtAction(nameof(GetBySlug), new { slug = page.Slug }, page);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{slug}")]
        public async Task<IActionResult> UpdateAsync(string slug, [FromBody] WikiPageUpdateRequest request)
        {
            try
            {
                var actionBool = _wikiPageService.UpdateWikiPage(slug, request);
                return Ok(actionBool);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
