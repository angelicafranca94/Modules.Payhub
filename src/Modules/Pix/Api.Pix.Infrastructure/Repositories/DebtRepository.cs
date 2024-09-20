using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Models;
using Api.Pix.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.Repositories;
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
