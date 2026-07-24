using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WikiDK.Services;

namespace WikiDK
{
    public class UserClaimsTransformation : IClaimsTransformation
    {
        private readonly UserCacheService _cache;
        private readonly UserService _userService;

        public UserClaimsTransformation(UserCacheService cache, UserService userService)
        {
            _cache = cache;
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (userId == 0)
                return principal;

            var user = await _cache.GetUserAsync($"user:{userId}", async () =>
            {
                return await _userService.GetById(userId);
            });
            if (user == null) return principal;

            var identity = (ClaimsIdentity?)principal.Identity;

            if (identity == null || !identity.IsAuthenticated)
                return principal;

            var roleClaim = identity.FindFirst(ClaimTypes.Role);
            identity.TryRemoveClaim(roleClaim);
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));


            return principal;
        }
    }
}
