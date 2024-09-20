
using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.Repositories;

public class BMRemessaRepository : IBMRemessaRepository
{
    private readonly BMRemessaContext _bMRemessaContext;


    public BMRemessaRepository(BMRemessaContext bMRemessaContext)
    {
        _bMRemessaContext = bMRemessaContext;
    }

    public async Task<int?> GetDebtCodeByTxIdAsync(string txid)
    {
        return await _bMRemessaContext.BMRemessa
            .Where(o => o.TXID == txid)
            .Select(o => o.FNDebitCode).FirstOrDefaultAsync();
    }

    public async Task<int?> GetDebtCodeByOurNumberAsync(string ourNumber)
    {
        return await _bMRemessaContext.BMRemessa
            .Where(o => EF.Functions.Like(o.OurNumberDetail, $"%{ourNumber}%"))
            .Select(o => o.FNDebitCode).FirstOrDefaultAsync();
    }
}
