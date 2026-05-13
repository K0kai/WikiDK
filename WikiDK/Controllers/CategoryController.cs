using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                return Ok(await _categoryService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Owner,Editor")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest request)
        {
            try
            {
                var category = await _categoryService.CreateCategory(request);
                return Ok(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Owner,Editor")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Owner,Editor")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateRequest CCR)
        {
            try
            {
                var category = await _categoryService.GetById(id);
                if (category == null)
                    throw new Exception("Category not found");
                category.Name = CCR.Name;
                category.Description = CCR.Description;
                category.Slug = CCR.Slug;
                category.Icon = CCR.Icon;
                _categoryService.UpdateCategory(category);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

    }

    public class CategoryCreateRequest()
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Slug { get; set; } = string.Empty;
        public string? Icon { get; set; } = null;
    }
}
