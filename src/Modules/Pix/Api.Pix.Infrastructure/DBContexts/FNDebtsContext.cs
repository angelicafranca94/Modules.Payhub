
using Api.Pix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.DBContexts;

public class FNDebtsContext(DbContextOptions<FNDebtsContext> options) : DbContext(options)
{
    public DbSet<FNDebtsModel> FNDebts { get; set; }
}