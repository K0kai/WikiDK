using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WikiDK.Objects;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [ApiController]
    [Route("api/ranks")]
    public class RankController : ControllerBase
    {
        private readonly RankService _rankService;
        private readonly CloudinaryService _cloudinaryService;

        public RankController(RankService rankService, CloudinaryService cloudinaryService)
        {
            _rankService = rankService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ranks = await _rankService.GetAllAsync();
            return Ok(ranks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rank = await _rankService.GetByIdAsync(id);
            if (rank == null) return NotFound();

            return Ok(rank);
        }
        [Authorize(Roles = "Editor,Admin,Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RankCreateDTO rankDTO)
        {
            var rank = new Rank()
            {
                Name = rankDTO.Name,
                Description = rankDTO.Description,
            };
            if (rankDTO.Icon != null && rankDTO.Icon.Length > 0)
            {
                var rankIcon = await _cloudinaryService.UploadImage(rankDTO.Icon);
                rank.Icon = rankIcon ?? "";
            }
            var created = await _rankService.CreateAsync(rank);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [Authorize(Roles = "Editor,Admin,Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rank rank)
        {
            var updated = await _rankService.UpdateAsync(id, rank);
            if (updated == null) return NotFound();

            return Ok(updated);
        }
        [Authorize(Roles = "Editor,Admin,Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _rankService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
    public class RankCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public IFormFile? Icon { get; set; }
    }
}
