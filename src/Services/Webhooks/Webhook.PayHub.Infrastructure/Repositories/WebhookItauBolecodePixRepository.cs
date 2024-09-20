using Webhook.PayHub.Application.Interfaces.Repositories;
using Webhook.PayHub.Domain.Models;
using Webhook.PayHub.Infrastructure.DBContexts;

namespace Webhook.PayHub.Infrastructure.Repositories;

public class WebhookItauBolecodePixRepository : IWebhookItauBolecodePixRepository
{
    private readonly PixWebhookItauContext _pixWebhookItauContext;

    public WebhookItauBolecodePixRepository(PixWebhookItauContext pixWebhookItauContext)
    {
        _pixWebhookItauContext = pixWebhookItauContext;
    }


    public async Task InsertAsync(WebhookItauBolecodePixModel pixWebhook)
    {
        await _pixWebhookItauContext.Pix.AddAsync(pixWebhook);
        await _pixWebhookItauContext.SaveChangesAsync();
    }
}