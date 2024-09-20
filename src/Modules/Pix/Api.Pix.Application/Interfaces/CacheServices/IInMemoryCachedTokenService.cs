namespace Api.Pix.Application.Interfaces.CacheServices;
public interface IInMemoryCachedTokenService
{
    string GetCachedToken(string key);
    string SetCachedToken(string key, string token, int expirationInMinutes);
}
