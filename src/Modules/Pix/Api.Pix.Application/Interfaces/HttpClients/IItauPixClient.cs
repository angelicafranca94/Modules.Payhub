using Api.Pix.Domain.Models;

namespace Api.Pix.Application.Interfaces.HttpClients;

public interface IItauPixClient
{
    Task<HttpResponseMessage> CreatePixAndTxIdAsync(int debtCode, string crtPath, string keyPath, string jsonOutput, AccountsBankModel accountData);
}
