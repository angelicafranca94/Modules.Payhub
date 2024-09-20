using Microsoft.EntityFrameworkCore;
using Webhook.PayHub.Domain.Models;
using Webhook.PayHub.Domain.Models.ResponseItauWebHook;

namespace Webhook.PayHub.Infrastructure.DBContexts;

public class PixWebhookItauContext(DbContextOptions<PixWebhookItauContext> options) : DbContext(options)
{
    public DbSet<WebhookItauBolecodePixModel> Pix { get; set; }
    public DbSet<ComponentsAmountModel> ComponentsAmounts { get; set; }
    public DbSet<DevolutionModel> Devolutions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebhookItauBolecodePixModel>()
            .HasKey(p => p.Code);

        modelBuilder.Entity<ComponentsAmountModel>()
            .HasOne(c => c.Pix)
        .WithOne(p => p.ComponentesAmount)
            .HasForeignKey<ComponentsAmountModel>(c => c.WebhookItauBolecodePixCode);

        modelBuilder.Entity<DevolutionModel>()
            .HasOne(d => d.Pix)
            .WithMany(p => p.Devolutions)
            .HasForeignKey(d => d.WebhookItauBolecodePixCode);


        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Original);
        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Loot);
        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Change);
        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Fees);
        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Fine);
        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Reduction);
        modelBuilder.Entity<ComponentsAmountModel>().OwnsOne(c => c.Discount);

        modelBuilder.Entity<DevolutionModel>().OwnsOne(d => d.Time);
    }
}