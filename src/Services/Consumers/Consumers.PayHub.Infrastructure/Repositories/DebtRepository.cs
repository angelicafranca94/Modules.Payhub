using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Domain.Models;
using Consumers.PayHub.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Consumers.PayHub.Infrastructure.Repositories;

public class DebtRepository : IDebtRepository
{
    private readonly FNDebtsContext _fNDebtsContext;

    public DebtRepository(FNDebtsContext fNDebtsContext)
    {
        _fNDebtsContext = fNDebtsContext;
    }

    public async Task<FNDebtsModel?> GetDebitosByCodeAsync(int debtCode)
    {
        return await _fNDebtsContext.FNDebts.FindAsync(debtCode);
    }

    public async Task PayDebtAsync(FNDebtsModel fNDebts)
    {
        _fNDebtsContext.Entry(fNDebts).State = EntityState.Modified;
        await _fNDebtsContext.SaveChangesAsync();
    }
}
