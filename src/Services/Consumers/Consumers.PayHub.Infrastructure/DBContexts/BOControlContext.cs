using Consumers.PayHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.DBContexts;
public class BOControlContext(DbContextOptions<BOControlContext> options) : DbContext(options)
{
    public DbSet<BOControlModel> BOControl { get; set; }
}