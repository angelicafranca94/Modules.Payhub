using Api.Pix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.DBContexts;

public class AccountsBankContext(DbContextOptions<AccountsBankContext> options) : DbContext(options)
{
    public DbSet<AccountsBankModel> AccountsBanks { get; set; }
}
