using Consumers.PayHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.DBContexts;
public class WebhookItauBolecodePixContext(DbContextOptions<WebhookItauBolecodePixContext> options) : DbContext(options)
{
    public DbSet<WebhookItauBolecodePixModel> PixWebhookItau { get; set; }
}