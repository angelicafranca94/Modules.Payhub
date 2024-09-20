using Api.Pix.Domain.Models;

namespace Api.Pix.Application.Interfaces.Repositories;
public interface IPixControlRepository
{
    Task InsertAsync(PixControlModel pix);
    Task<PixControlModel?> GetPixControlByTxIdAsync(string txid);
    Task UpdateAsync(PixControlModel pixObj);
    Task<PixControlModel?> GetPixControlByDebtIdAndActiveAsync(int debtCode);
}
