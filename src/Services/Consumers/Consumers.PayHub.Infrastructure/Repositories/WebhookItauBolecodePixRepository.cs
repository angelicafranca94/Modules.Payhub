using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Domain.Models;
using Consumers.PayHub.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.Repositories;

public class WebhookItauBolecodePixRepository : IWebhookItauBolecodePixRepository
{
    private readonly WebhookItauBolecodePixContext _pixWebhookItauContext;

    public WebhookItauBolecodePixRepository(WebhookItauBolecodePixContext pixWebhookItauContext)
    {
        _pixWebhookItauContext = pixWebhookItauContext;
    }


    public async Task InsertAsync(WebhookItauBolecodePixModel pixWebhook)
    {
        await _pixWebhookItauContext.PixWebhookItau.AddAsync(pixWebhook);
        await _pixWebhookItauContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(WebhookItauBolecodePixModel pixWebhook)
    {
        _pixWebhookItauContext.Entry(pixWebhook).State = EntityState.Modified;
        await _pixWebhookItauContext.SaveChangesAsync();
    }

    public async Task<WebhookItauBolecodePixModel?> GetByTxIdAsync(string txId)
    {
        return await _pixWebhookItauContext.PixWebhookItau.Where(w => w.Txid == txId)
              .FirstOrDefaultAsync();
    }
}
