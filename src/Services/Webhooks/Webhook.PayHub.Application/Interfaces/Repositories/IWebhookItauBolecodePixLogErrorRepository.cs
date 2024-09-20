using Webhook.PayHub.Domain.Models;

namespace Webhook.PayHub.Application.Interfaces.Repositories;
public interface IWebhookItauBolecodePixLogErrorRepository
{
    Task InsertAsync(WebhookItauBolecodePixLogErrorModel logError);
}
