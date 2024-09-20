using CrossCutting.PayHub.Shared.Dtos.Messages;

namespace Consumers.PayHub.Application.Interfaces.Services;
public interface IPixBolecodePaymentService
{
    Task DebtPaymentProcessAsync(ItauWebhookMessage itauWebhook);
}
