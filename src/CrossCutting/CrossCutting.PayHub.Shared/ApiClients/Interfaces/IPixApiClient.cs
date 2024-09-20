using CrossCutting.PayHub.Shared.Dtos.Messages;

namespace CrossCutting.PayHub.Shared.ApiClients.Interfaces;

public interface IPixApiClient
{
    Task DebtPaymentAsync(ItauWebhookMessage itauWebhook);
}
