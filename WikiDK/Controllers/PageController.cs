using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [Route("api/pages")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private PageService pageService;
        public PageController(PageService pageService)
        {
            this.pageService = pageService;
        }
        [Authorize(Roles = "Editor, Admin, Owner")]
        [HttpPost("section")]
        public async Task<IActionResult> CreatePageSection([FromBody] PageSectionCreateRequest request)
        {
            try
            {
                var pageSection = await pageService.CreateSection(request);
                return CreatedAtAction(nameof(GetPageSection), new { id = pageSection.Id }, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Editor, Admin, Owner")]
        [HttpPatch("section/{id}")]
        public async Task<IActionResult> UpdatePageSection([FromRoute] int id, [FromBody] PageSectionCreateRequest request)
        {
            try
            {
                var section = await pageService.UpdateSection(id, request);
                return Ok(section);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Owner")]
        [HttpPatch("section/regen/slug")]
        public async Task<IActionResult> RegenerateSectionSlug([FromQuery] int id)
        {
            try
            {
                var section = await pageService.RegenSlug(id);
                return Ok(section);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("section/{id}")]
        public async Task<IActionResult> GetPageSection([FromRoute] int id)
        {
            try
            {
                var pageSection = await pageService.GetSection(id);
                return Ok(pageSection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("section")]
        public async Task<IActionResult> GetPageSections()
        {
            try
            {
                var pageSections = await pageService.GetSections();
                return Ok(pageSections);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("section/limit")]
        public async Task<IActionResult> GetSectionLimit()
        {
            try
            {
                var limit = pageService.GetSectionLimit();
                return Ok(limit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Editor, Admin, Owner")]
        [HttpPost("section/reorder")]
        public async Task<IActionResult> ReorderSections([FromBody] List<ReorderRequest> requests)
        {
            try
            {
                var sections = await pageService.ReorderSections(requests);
                return Ok(sections);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }
    public class PageSectionCreateRequest()
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Order { get; set; }
        public bool IsVisible { get; set; } = true;
    }
    public class ReorderRequest()
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }
}
