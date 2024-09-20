using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Models;
using Api.Pix.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.Repositories;
public class PixControlRepository : IPixControlRepository
{
    private readonly PixControlContext _pixControlContext;

    public PixControlRepository(PixControlContext pixControlContext)
    {
        _pixControlContext = pixControlContext;
    }

    public async Task InsertAsync(PixControlModel pix)
    {
        _pixControlContext.PixControl.Add(pix);
        await _pixControlContext.SaveChangesAsync();
    }

    public async Task<PixControlModel?> GetPixControlByTxIdAsync(string txid)
    {
        return await _pixControlContext.PixControl.FindAsync(txid);
    }

    public async Task UpdateAsync(PixControlModel pixObj)
    {
        _pixControlContext.Entry(pixObj).State = EntityState.Modified;
        await _pixControlContext.SaveChangesAsync();
    }

    public async Task<PixControlModel?> GetPixControlByDebtIdAndActiveAsync(int debtCode)
    {
        return await _pixControlContext.PixControl
             .Where(o => o.OriginSystemIdentifierCode == debtCode)
             .OrderByDescending(o => o.DateTimeRegistration)
             .FirstOrDefaultAsync();

    }
}
