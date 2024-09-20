using Consumers.PayHub.Domain.Models;

namespace Consumers.PayHub.Application.Interfaces.Repositories;
public interface IPixControlRepository
{
    Task InsertAsync(PixControlModel pix);
    Task<PixControlModel?> GetPixControlByTxIdAsync(string txid);
    Task UpdateAsync(PixControlModel pixObj);
    Task<PixControlModel?> GetPixControlByDebtIdAndActiveAsync(int debtCode);
}
