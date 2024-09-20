using Consumers.PayHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.DBContexts;
public class FNDebtsContext(DbContextOptions<FNDebtsContext> options) : DbContext(options)
{
    public DbSet<FNDebtsModel> FNDebts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FNDebtsModel>()
            .ToTable(tb => tb.HasTrigger("trFNDebitos"));
    }
}