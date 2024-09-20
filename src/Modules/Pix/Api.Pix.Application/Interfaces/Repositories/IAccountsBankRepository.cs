using Api.Pix.Domain.Models;

namespace Api.Pix.Application.Interfaces.Repositories;
public interface IAccountsBankRepository
{
    Task<AccountsBankModel?> GetPixKeyByCode(int code);
}
