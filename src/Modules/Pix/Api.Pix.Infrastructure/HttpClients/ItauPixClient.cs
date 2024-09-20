
using Api.Pix.Application.Interfaces.CacheServices;
using Api.Pix.Application.Interfaces.HttpClients;
using Api.Pix.Domain.Interfaces;
using Api.Pix.Domain.Models;
using Api.Pix.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Pix.Infrastructure.HttpClients;
public class ItauPixClient(
     IExternalApiService externalApiService, IInMemoryCachedTokenService cachedTokenService, IOptions<MemoryCachedSettings> memoryCachedSettings,
     ILogger<ItauPixClient> logger
        ) : IItauPixClient
{
    private readonly IExternalApiService _externalApiService = externalApiService;
    private readonly IInMemoryCachedTokenService _cachedTokenService = cachedTokenService;
    private readonly MemoryCachedSettings _memoryCachedSettings = memoryCachedSettings.Value;
    private readonly ILogger<ItauPixClient> _logger = logger;

    public async Task<HttpResponseMessage> CreatePixAndTxIdAsync(int debtCode, string crtPath, string keyPath, string jsonOutput, AccountsBankModel accountData)
    {
        var token = await CheckValidityAndReturnTokenAsync(crtPath, keyPath, accountData);

        return await _externalApiService.GeneratePixAsync(debtCode, token, crtPath, keyPath, jsonOutput, accountData.ClientId, accountData.ClientSecret);

    }

    private async Task<string> CheckValidityAndReturnTokenAsync(string crtPath, string keyPath, AccountsBankModel accountData)
    {
        var cachedToken = _cachedTokenService.GetCachedToken(_memoryCachedSettings.Key);

        if (!string.IsNullOrEmpty(cachedToken))
        {
            _logger.LogInformation("Returning token from memory cache");
            return cachedToken;
        }

        return await _externalApiService.GenerateTokenAsync(crtPath, keyPath, accountData.ClientId, accountData.ClientSecret);
    }
}
