using Microsoft.Extensions.Caching.Memory;
using WikiDK.Objects;

namespace WikiDK.Services
{
    public class UserCacheService
    {
        private readonly IMemoryCache _cache;

        public UserCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<User?> GetUserAsync(string userId, Func<Task<User?>> factory)
        {
            return await _cache.GetOrCreateAsync(userId, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await factory();
            });
        }

        public void RemoveUser(string userId)
        {
            _cache.Remove(userId);
        }


    }
}
