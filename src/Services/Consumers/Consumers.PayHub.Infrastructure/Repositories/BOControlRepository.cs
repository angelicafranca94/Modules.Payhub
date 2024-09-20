
using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Infrastructure.DBContexts;
using CrossCutting.PayHub.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.Repositories;

public class BOControlRepository : IBOControlRepository
{
    private readonly BOControlContext _bOControlContext;

    public BOControlRepository(BOControlContext bOControlContext)
    {
        _bOControlContext = bOControlContext;
    }

    public async Task<int?> GetDebtCodeByOurNumberAsync(string ourNumber)
    {
        return await _bOControlContext.BOControl
           .Where(o => EF.Functions.Like(o.OurNumber, $"%{ourNumber}%")
           && o.OriginSystemTable == DataBaseContants.FnDebts)
           .Select(o => o.OriginSystemId).FirstOrDefaultAsync();
    }
}
