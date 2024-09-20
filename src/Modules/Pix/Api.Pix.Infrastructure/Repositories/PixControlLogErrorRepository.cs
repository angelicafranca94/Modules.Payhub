using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Models;
using Api.Pix.Infrastructure.DBContexts;

namespace Api.Pix.Infrastructure.Repositories;
public class PixControlLogErrorRepository : IPixControlLogErrorRepository
{
    private readonly PixControlLogErrorContext _pixControlLogErrorContext;

    public PixControlLogErrorRepository(PixControlLogErrorContext pixControlLogErrorContext)
    {
        _pixControlLogErrorContext = pixControlLogErrorContext;
    }

    public async Task InsertAsync(PixControlLogErrorModel logError)
    {
        _pixControlLogErrorContext.PixControlLogError.Add(logError);
        await _pixControlLogErrorContext.SaveChangesAsync();
    }
}
