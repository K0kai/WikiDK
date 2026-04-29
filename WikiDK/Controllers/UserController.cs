using Microsoft.AspNetCore.Mvc;

namespace WikiDK.Controllers
{
    [ApiController]
    [Route("[users]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            
            return Ok(new { Message = "User registered successfully", User = request });
        }
    }
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
