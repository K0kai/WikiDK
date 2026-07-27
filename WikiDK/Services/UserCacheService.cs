using Microsoft.Extensions.Caching.Memory;
using WikiDK.Objects;

namespace WikiDK.Services
{
    public class UserCacheService(IMemoryCache cache)
    {
        public async Task<User?> GetUserAsync(string userId, Func<Task<User?>> factory)
        {
            return await cache.GetOrCreateAsync(userId, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await factory();
            });
        }

        public void RemoveUser(string userId)
        {
            cache.Remove(userId);
        }


    }
}
