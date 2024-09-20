namespace Api.Pix.Domain.Interfaces;
public interface IResiliencePolicy
{
    Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action);
}