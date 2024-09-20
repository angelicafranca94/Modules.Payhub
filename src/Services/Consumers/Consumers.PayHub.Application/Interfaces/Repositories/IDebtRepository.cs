using Consumers.PayHub.Domain.Models;

namespace Consumers.PayHub.Application.Interfaces.Repositories;

public interface IDebtRepository
{
    Task<FNDebtsModel?> GetDebitosByCodeAsync(int debtCode);
    Task PayDebtAsync(FNDebtsModel fNDebts);

}
