using Consumers.PayHub.Application.Interfaces.Services;
using CrossCutting.PayHub.Shared.Dtos.Messages;
using MassTransit;

namespace Consumers.Service.Consumers;
public class ItauPixDebtPaymentConsumer : IConsumer<ItauWebhookMessage>
{
    private readonly IPixBolecodePaymentService _paymentService;

    public ItauPixDebtPaymentConsumer(IPixBolecodePaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task Consume(ConsumeContext<ItauWebhookMessage> context)
    {
        await _paymentService.DebtPaymentProcessAsync(context.Message);
    }
}
