namespace CrossCutting.PayHub.Shared.Dtos.Messages;

public class ItauWebhookMessage
{
    public string EndToEndId { get; set; }
    public string Txid { get; set; }
    public string Amount { get; set; }
    public string Timestamp { get; set; }
    public string InfoPay { get; set; }
    public string? Key { get; set; }
}
