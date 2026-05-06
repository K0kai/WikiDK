using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [ApiController]
    [Route("users")]
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
                var user = _authService.Register(request.Name, request.Email, request.Password);
                _ = _userService.Create(user).Result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }            
            return Ok(new { Message = "User registered successfully", User = request });
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = _authService.Login(request.Name, request.Password);
                return Ok(new { Message = "User logged in successfully", Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("get/me")]
        public IActionResult GetMe()
        {
            try
            {
                var validId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id);
                if (!validId)
                    return BadRequest(new { Message = "User ID not found in token" });

                var user = _userService.GetById(id).Result;
                return Ok(new { Message = "User retrieved successfully", User = user });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in get/me endpoint: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
