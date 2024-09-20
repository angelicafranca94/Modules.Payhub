using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Models;
using Api.Pix.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Pix.Infrastructure.Repositories;

public class AccountsBankRepository : IAccountsBankRepository
{
    private readonly AccountsBankContext _accountsBankContext;

    public AccountsBankRepository(AccountsBankContext accountsBankContext)
    {
        _accountsBankContext = accountsBankContext;
    }

    public async Task<AccountsBankModel?> GetPixKeyByCode(int code)
    {
        return await _accountsBankContext.AccountsBanks.Where(a => a.Code == code).FirstOrDefaultAsync();
    }
}
