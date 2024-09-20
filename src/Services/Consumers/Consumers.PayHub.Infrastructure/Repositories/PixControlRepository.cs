using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Domain.Models;
using Consumers.PayHub.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
namespace Consumers.PayHub.Infrastructure.Repositories;

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
        return await _pixControlContext.PixControl.Where(p => p.TransactionId == txid).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(PixControlModel pixObj)
    {
        _pixControlContext.Entry(pixObj).State = EntityState.Modified;
        await _pixControlContext.SaveChangesAsync();
    }

    public async Task<PixControlModel?> GetPixControlByDebtIdAndActiveAsync(int debtCode)
    {
        return await _pixControlContext.PixControl
             .Where(o => o.OriginSystemIdentifierCode == debtCode).FirstOrDefaultAsync();

    }
}
