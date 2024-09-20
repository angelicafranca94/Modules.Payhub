
using Api.Pix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.DBContexts;

public class PixControlContext(DbContextOptions<PixControlContext> options) : DbContext(options)
{
    public DbSet<PixControlModel> PixControl { get; set; }
}