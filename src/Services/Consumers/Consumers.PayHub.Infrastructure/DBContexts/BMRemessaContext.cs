using Consumers.PayHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.DBContexts;
public class BMRemessaContext(DbContextOptions<BMRemessaContext> options) : DbContext(options)
{
    public DbSet<BMRemessaPixModel> BMRemessa { get; set; }
}
