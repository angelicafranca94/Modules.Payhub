using Microsoft.EntityFrameworkCore;
using Webhook.PayHub.Domain.Models;

namespace Webhook.PayHub.Infrastructure.DBContexts;

public class WebhookItauBolecodePixLogErrorContext(DbContextOptions<WebhookItauBolecodePixLogErrorContext> options) : DbContext(options)
{
    public DbSet<WebhookItauBolecodePixLogErrorModel> WebhookItauBolecodePixLogErrors { get; set; }
}
