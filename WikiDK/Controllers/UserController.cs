using Microsoft.AspNetCore.Mvc;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [ApiController]
    [Route("[users]")]
    public class UserController : ControllerBase
    {
        private AuthService _authService {  get; set; }
        private UserService _userService { get; set; }

        public UserController(AuthService authSvc, UserService userSvc)
        {
            _authService = authSvc;
            _userService = userSvc;
        } 

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                _authService.Register(request.Name, request.Email, request.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }            
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
