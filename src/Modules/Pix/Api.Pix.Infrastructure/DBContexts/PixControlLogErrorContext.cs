
using Api.Pix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.DBContexts;

public class PixControlLogErrorContext(DbContextOptions<PixControlLogErrorContext> options) : DbContext(options)
{
    public DbSet<PixControlLogErrorModel> PixControlLogError { get; set; }
}