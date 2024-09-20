using Api.Pix.Domain.Models;

namespace Api.Pix.Application.Interfaces.Repositories;
public interface IPixControlLogErrorRepository
{
    Task InsertAsync(PixControlLogErrorModel logError);
}
