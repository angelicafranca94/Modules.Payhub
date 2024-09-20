using Webhook.PayHub.Domain.Models;

namespace Webhook.PayHub.Application.Interfaces.Repositories;
public interface IWebhookItauBolecodePixRepository
{
    Task InsertAsync(WebhookItauBolecodePixModel pixWebhook);
}
