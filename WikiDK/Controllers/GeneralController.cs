using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WikiDK.Controllers
{
    [Route("api/general")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        [HttpGet("health")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHealth()
        {
            return Ok("OK");
        }
    }
}
