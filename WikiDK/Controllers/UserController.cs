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
        private AuthService _authService { get; set; }
        private UserService _userService { get; set; }
        private CloudinaryService _cloudinaryService { get; set; }

        public UserController(AuthService authSvc, UserService userSvc, CloudinaryService cloudinaryService)
        {
            _authService = authSvc;
            _userService = userSvc;
            _cloudinaryService = cloudinaryService;
        }
        /// <summary>
        /// API Endpoint for an user to register themselves on the platform.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        /// <summary>
        /// API Endpoint for an user to perform login on the platform.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        /// <summary>
        /// API Endpoint to return information about the currently logged in user.
        /// </summary>
        /// <returns></returns>
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
                return BadRequest(new { ex.Message });
            }
        }
        /// <summary>
        /// API Endpoint for an user to edit their own information.
        /// </summary>
        /// <param name="UIR"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("edit/me")]
        public async Task<IActionResult> EditMe([FromBody] UpdateInfoRequest UIR)
        {
            try
            {
                var validId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id);
                if (!validId)
                    return BadRequest(new { Message = "Invalid token" });

                var newUser = await _userService.Update(id, UIR);

                return Ok(new { Message = "User updated successfully", User = newUser });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// API Endpoint to change a user's icon picture, utilizes Cloudinary.
        /// </summary>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upload/icon")]
        public async Task<IActionResult> UploadUserPhoto(IFormFile imgFile)
        {
            try
            {
                var validId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id);
                if (!validId)
                    return BadRequest(new { Message = "Invalid token" });

                var user = await _userService.GetById(id) ?? throw new Exception("User does not exist");

                if (imgFile == null || imgFile.Length == 0)
                    throw new Exception("Sent image doesn't exist");
                if (imgFile.Length >= 5 * 1024 * 1024)
                {
                    return BadRequest("File too large");
                }
                var allowedTypes = new[]
                {
                "image/png",
                "image/jpeg",
                "image/webp"
                };

                var extension =
    Path.GetExtension(imgFile.FileName).ToLower();

                var allowedExtensions =
                    new[] { ".png", ".jpg", ".jpeg", ".webp" };

                if (!allowedTypes.Contains(imgFile.ContentType))
                    return BadRequest("Invalid file type");

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Unsupported extension");

                var url = await _cloudinaryService.UploadImage(imgFile);

                user.UserIcon = $"{url}";

                await _userService.Update(user);

                return Ok(url);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in upload/icon endpoint: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }
        /// <summary>
        /// API Endpoint to remove an user's icon and revert it to the default state.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("remove/icon")]
        public async Task<IActionResult> RemoveIcon()
        {
            try
            {
                var validId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id);
                if (!validId)
                    return BadRequest(new { Message = "User ID not found in token" });

                var user = await _userService.GetById(id) ?? throw new Exception("User doesn't exist");

                user.UserIcon = ServerDefaults.DefaultUserIcon; ;

                await _userService.Update(user);

                return Ok(user.UserIcon);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// API Endpoint to return the server's current default icon for users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("default/icon")]
        public async Task<IActionResult> GetDefaultIcon()
        {
            return Ok(ServerDefaults.DefaultUserIcon);
        }
    }
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UpdateInfoRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
    public class LoginRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
