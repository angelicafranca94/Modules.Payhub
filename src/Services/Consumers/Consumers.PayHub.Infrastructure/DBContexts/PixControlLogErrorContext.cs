using Consumers.PayHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.DBContexts;
public class PixControlLogErrorContext(DbContextOptions<PixControlLogErrorContext> options) : DbContext(options)
{
    public DbSet<PixControlLogErrorModel> PixControlLogError { get; set; }
}