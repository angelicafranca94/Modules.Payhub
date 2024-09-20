using Consumers.PayHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.DBContexts;
public class PixControlContext(DbContextOptions<PixControlContext> options) : DbContext(options)
{
    public DbSet<PixControlModel> PixControl { get; set; }
}