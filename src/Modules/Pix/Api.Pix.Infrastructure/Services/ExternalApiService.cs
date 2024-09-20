using Api.Pix.Application.Interfaces.CacheServices;
using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Interfaces;
using Api.Pix.Domain.Models;
using Api.Pix.Domain.Models.Responses;
using Api.Pix.Infrastructure.Exceptions;
using Api.Pix.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Api.Pix.Infrastructure.Services;

public class ExternalApiService : IExternalApiService
{
    private readonly IResiliencePolicy _resiliencePolicy;
    private readonly IInMemoryCachedTokenService _cachedTokenService;
    private readonly MemoryCachedSettings _memoryCachedSettings;
    private readonly PixSettings _pixSettings;
    private readonly GenerateTokenPixSettings _generatePixTokenSettings;
    private readonly PixUriSettings _pixUriSettings;
    private readonly ILogger<ExternalApiService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPixControlLogErrorRepository _logError;

    public ExternalApiService(IResiliencePolicy resiliencePolicy,
        IInMemoryCachedTokenService cachedTokenService,
        IOptions<MemoryCachedSettings> memoryCachedSettings,
        IOptions<PixSettings> pixSettings,
        IOptions<GenerateTokenPixSettings> generatePixTokenSettings,
        IOptions<PixUriSettings> pixUriSettings,
        ILogger<ExternalApiService> logger,
        IHttpContextAccessor httpContextAccessor,
        IPixControlLogErrorRepository logError)
    {
        _resiliencePolicy = resiliencePolicy;
        _cachedTokenService = cachedTokenService;
        _memoryCachedSettings = memoryCachedSettings.Value;
        _pixSettings = pixSettings.Value;
        _generatePixTokenSettings = generatePixTokenSettings.Value;
        _pixUriSettings = pixUriSettings.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _logError = logError;
    }

    public async Task<string> GenerateTokenAsync(string crtPath, string keyPath, string clientId, string clientSecret)
    {
        ValidateParameters(crtPath, keyPath, clientId, clientSecret);

        using var client = CreateHttpClientWithCertificate(crtPath, keyPath);
        SetDefaultHeaders(client);

        var headers = GetCommonItemsHeader(clientId, clientSecret);
        headers.Add(new KeyValuePair<string, string>("grant_type", _pixSettings.GrantType));
        var requestBodyString = new FormUrlEncodedContent(headers);
        var uri = $"{_generatePixTokenSettings.BaseUrl}{_generatePixTokenSettings.Path}";

        return await SendRequestAsync(() => new HttpRequestMessage(HttpMethod.Post, uri) { Content = requestBodyString }, client, HandleTokenResponse);

    }

    private async Task<string> HandleTokenResponse(HttpResponseMessage response)
    {
        var accessTokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>()
            ?? throw new Exception("Failed to parse token response for token.");

        _cachedTokenService.SetCachedToken(_memoryCachedSettings.Key, accessTokenResponse.AccessToken, accessTokenResponse.ExpiresIn);

        _logger.LogInformation("Returning token from request");
        return accessTokenResponse.AccessToken;
    }

    public async Task<HttpResponseMessage> GeneratePixAsync(int debtCode, string token, string crtPath, string keyPath, string jsonOutput, string clientId, string clientSecret)
    {
        ValidateParameters(crtPath, keyPath, clientId, clientSecret);
        if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
        if (string.IsNullOrEmpty(jsonOutput)) throw new ArgumentNullException(nameof(jsonOutput));

        using var client = CreateHttpClientWithCertificate(crtPath, keyPath);
        AddHeaders(client, token, clientId);

        var uri = $"{_pixUriSettings.BaseUrl}{_pixUriSettings.QrCodeImediatoPath}";
        var stringRequestContent = new StringContent(jsonOutput, Encoding.UTF8, "application/json");

        _httpContextAccessor.HttpContext.Items["debtCode"] = debtCode;

        return await SendRequestAsync(() => new HttpRequestMessage(HttpMethod.Post, uri) { Content = stringRequestContent }, client, response => Task.FromResult(response));
    }

    private HttpClient CreateHttpClientWithCertificate(string crtPath, string keyPath)
    {
        _logger.LogInformation("Creating HttpClient with client certificate.");
        var certificate = AddCertificate(crtPath, keyPath);

        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);
        return new HttpClient(handler);
    }

    private static X509Certificate2 AddCertificate(string crtPath, string keyPath)
    {
        X509Certificate2 certWithKey = X509Certificate2.CreateFromPemFile(crtPath, keyPath);

        string senhaExportPfx = Guid.NewGuid().ToString();

        byte[] crtRawData = certWithKey.Export(X509ContentType.Pfx, senhaExportPfx);

        return new X509Certificate2(crtRawData, senhaExportPfx);
    }

    private static void AddHeaders(HttpClient client, string token, string clientId)
    {
        client.DefaultRequestHeaders.Add("x-itau-apikey", clientId);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        client.DefaultRequestHeaders.Add("x-itau-correlationID", Guid.NewGuid().ToString());
        client.DefaultRequestHeaders.Add("x-itau-flowID", Guid.NewGuid().ToString());
    }

    private List<KeyValuePair<string, string>> GetCommonItemsHeader(string clientId, string clientSecret)
    {
        return
        [
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        ];
    }


    private async Task<T> SendRequestAsync<T>(Func<HttpRequestMessage> requestMessageFactory, HttpClient client, Func<HttpResponseMessage, Task<T>> onSuccess)
    {
        string responseContent = string.Empty;

        try
        {
            _logger.LogInformation("Sending request...");

            var response = await _resiliencePolicy.ExecuteAsync(() => client.SendAsync(requestMessageFactory()));

            responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("HTTP request failed with status code {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                throw new RequestFailedException(response.StatusCode, responseContent);
            }

            _logger.LogInformation("Received HTTP response: {Response}", responseContent);
            return await onSuccess(response);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, requestMessageFactory(), responseContent);
            throw;
        }
    }

    private async Task HandleExceptionAsync(Exception ex, HttpRequestMessage request, string responseContent)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var txid = httpContext?.Items["txid"] as string ?? string.Empty;
        var debtCode = httpContext?.Items["debtCode"] is int code ? code : 0;

        var logErrorObject = CreateLogErrorObject(ex, debtCode, txid, request.ToString(), responseContent);
        await _logError.InsertAsync(logErrorObject);

        _logger.LogError(ex, "Exception caught in HTTP request.");
    }

    private static PixControlLogErrorModel CreateLogErrorObject(Exception ex, int debtCode, string txid, string jsonOutput, string jsonInput)
    {
        var logErrorObject = new PixControlLogErrorModel()
        {
            ErrorMessage = ex.Message ?? string.Empty,
            StackTrace = ex.StackTrace ?? string.Empty,
            DebtCode = debtCode,
            TransactionId = txid,
            JsonOutput = jsonOutput,
            JsonInput = jsonInput
        };

        return logErrorObject;
    }

    private static void ValidateParameters(params string[] parameters)
    {
        foreach (var parameter in parameters)
        {
            if (string.IsNullOrEmpty(parameter))
                throw new ArgumentNullException(parameter);
        }
    }

    private static void SetDefaultHeaders(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("x-itau-correlationID", Guid.NewGuid().ToString());
        client.DefaultRequestHeaders.Add("x-itau-flowID", Guid.NewGuid().ToString());
    }
}