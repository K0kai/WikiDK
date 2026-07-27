using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WikiDK.Objects;
using WikiDK.Services;
using WikiDK.Services.Interfaces;

namespace WikiDK.Controllers
{
    [Route("api/wiki")]
    [ApiController]
    public class WikiPageController (IWikiPageService wikiPageService, UserService userService) : ControllerBase
    {

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            try
            {
                var page = await wikiPageService.GetWikiPageBySlug(slug);
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
                _ = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
                var user = await userService.GetById(userId);
                request.Author = user;
                var page = await wikiPageService.CreateWikiPageAsync(request);
                return CreatedAtAction(nameof(GetBySlug), new { slug = page.Slug }, page);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{param}")]
        public async Task<IActionResult> UpdateAsync(string param, [FromBody] WikiPageUpdateRequest request)
        {
            try
            {
                // Attempts to convert the param to integer, if its successful then it means the param is not a slug and is in fact an ID
                var isId = int.TryParse(param, out var id);
                _ = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
                var user = await userService.GetById(userId);
                request.Editor = user;
                // Handles both cases, if its a slug resolves to slug method variation, else resolves to id method variation
                var actionBool = isId ? wikiPageService.UpdateWikiPage(id, request) : wikiPageService.UpdateWikiPage(param, request);
                return Ok(actionBool);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
