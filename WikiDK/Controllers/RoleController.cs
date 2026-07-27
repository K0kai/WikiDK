using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WikiDK.Objects;
using WikiDK.Services;
using WikiDK.Services.Interfaces;

namespace WikiDK.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController(IRoleService roleService, UserService userService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await roleService.GetRoleById(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateRequest request)
        {
            _ = int.TryParse(User.FindFirstValue(claimType: ClaimTypes.NameIdentifier), out var userId);
            var user = await userService.GetById(userId);
            if (user == null)
                return BadRequest("Invalid user");
            request.User = user;
            _ = await roleService.CreateRole(request);
            return NoContent();
        }
    }
}
