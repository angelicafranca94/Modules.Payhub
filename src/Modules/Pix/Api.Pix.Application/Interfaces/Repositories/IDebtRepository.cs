using Api.Pix.Domain.Models;

namespace Api.Pix.Application.Interfaces.Repositories;

public interface IDebtRepository
{
    Task<FNDebtsModel?> GetDebitosByCodeAsync(int debtCode);
    Task PayDebtAsync(FNDebtsModel fNDebts);

}
