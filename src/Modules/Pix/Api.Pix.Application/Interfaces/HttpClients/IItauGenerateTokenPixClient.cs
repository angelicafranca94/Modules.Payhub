namespace Api.Pix.Application.Interfaces.HttpClients;

public interface IItauGenerateTokenPixClient
{
    Task<string> GenerateTokenAsync(string cnpj, string clientId, string clientSecret);
}
