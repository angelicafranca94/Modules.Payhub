using Api.Pix.Domain.Models.PixObjects;
using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.Payloads;
public class QRCodeImmediatePayload
{
    [JsonPropertyName("valor")]
    public AmountQRCodeImmediate Amount { get; set; }

    [JsonPropertyName("chave")]
    public string Key { get; set; }
}
