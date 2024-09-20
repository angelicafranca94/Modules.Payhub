using Consumers.PayHub.Domain.Models;

namespace Consumers.PayHub.Application.Interfaces.Repositories;

public interface IWebhookItauBolecodePixRepository
{
    Task InsertAsync(WebhookItauBolecodePixModel pixWebhook);
    Task<WebhookItauBolecodePixModel?> GetByTxIdAsync(string txId);
    Task UpdateAsync(WebhookItauBolecodePixModel pixWebhook);
}
