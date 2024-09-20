using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.PixObjects;

public class AmountQRCodeImmediate
{
    [JsonPropertyName("original")]
    public string Original { get; set; }
}
