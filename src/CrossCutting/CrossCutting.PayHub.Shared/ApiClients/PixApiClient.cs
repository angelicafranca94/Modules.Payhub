using CrossCutting.PayHub.Shared.ApiClients.Interfaces;
using CrossCutting.PayHub.Shared.Dtos.Messages;

namespace CrossCutting.PayHub.Shared.ApiClients;

public class PixApiClient(
    IHttpClientFactory clientFactory
) : IPixApiClient
{
    private readonly HttpClient _client = clientFactory.CreateClient("Pix");

    public async Task DebtPaymentAsync(ItauWebhookMessage itauWebhook)
    {
        await RequestBuilder
            .Post
            .WithUrl("/api/pixdebtpayment/v1/updatedebtpayment")
            .WithPayload(itauWebhook)
            .SendAsync(_client);
    }
}
