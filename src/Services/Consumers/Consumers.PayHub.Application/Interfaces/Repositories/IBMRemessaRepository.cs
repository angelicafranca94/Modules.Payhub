namespace Consumers.PayHub.Application.Interfaces.Repositories;
public interface IBMRemessaRepository
{
    Task<int?> GetDebtCodeByOurNumberAsync(string txid);

    Task<int?> GetDebtCodeByTxIdAsync(string txid);
}
