using Api.Pix.Application.Interfaces.CacheServices;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Pix.Infrastructure.Services;
public class InMemoryCachedTokenService : IInMemoryCachedTokenService
{
    private readonly IMemoryCache _memoryCache;

    public InMemoryCachedTokenService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

    public string GetCachedToken(string key)
    {
        var hasCachedToken = _memoryCache.TryGetValue(key, out string? cachedValue);
        return hasCachedToken ? cachedValue! : string.Empty;
    }

    public string SetCachedToken(string key, string token, int expirationInMinutes) =>
        _memoryCache.Set(key, token, options: new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(expirationInMinutes)
        });
}
