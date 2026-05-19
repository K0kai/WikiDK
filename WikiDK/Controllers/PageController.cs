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
        [Authorize(Roles ="Editor, Admin, Owner")]
        [HttpPost("section")]
        public async Task<IActionResult> CreatePageSection([FromBody] PageSectionCreateRequest request)
        {
            try
            {
                var pageSection = await pageService.CreateSection(request);
                return CreatedAtAction(nameof(GetPageSection), pageSection.Id, request);
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
    }
    public class PageSectionCreateRequest()
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Order { get; set; }
        public bool isVisible { get; set; } = true;
    }
}
