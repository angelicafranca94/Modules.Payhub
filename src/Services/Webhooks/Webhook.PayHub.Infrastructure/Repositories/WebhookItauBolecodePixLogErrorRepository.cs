using Webhook.PayHub.Application.Interfaces.Repositories;
using Webhook.PayHub.Domain.Models;
using Webhook.PayHub.Infrastructure.DBContexts;

namespace Webhook.PayHub.Infrastructure.Repositories;

public class WebhookItauBolecodePixLogErrorRepository : IWebhookItauBolecodePixLogErrorRepository
{
    private readonly WebhookItauBolecodePixLogErrorContext _webhookItauBolecodePixLogErrorContext;
    public WebhookItauBolecodePixLogErrorRepository(WebhookItauBolecodePixLogErrorContext webhookItauBolecodePixLogErrorContext)
    {
        _webhookItauBolecodePixLogErrorContext = webhookItauBolecodePixLogErrorContext;
    }
    public async Task InsertAsync(WebhookItauBolecodePixLogErrorModel logError)
    {
        _webhookItauBolecodePixLogErrorContext.WebhookItauBolecodePixLogErrors.Add(logError);
        await _webhookItauBolecodePixLogErrorContext.SaveChangesAsync();
    }
}
