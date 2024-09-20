namespace Consumers.PayHub.Application.Interfaces.Repositories;
public interface IBOControlRepository
{
    Task<int?> GetDebtCodeByOurNumberAsync(string ourNumber);
}
