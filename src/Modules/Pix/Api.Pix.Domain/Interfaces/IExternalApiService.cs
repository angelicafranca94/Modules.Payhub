namespace Api.Pix.Domain.Interfaces;
public interface IExternalApiService
{
    Task<HttpResponseMessage> GeneratePixAsync(int debtCode, string token, string crtPath, string keyPath, string jsonOutput, string clientId, string clientSecret);
    Task<string> GenerateTokenAsync(string crtPath, string keyPath, string clientId, string clientSecret);
}

