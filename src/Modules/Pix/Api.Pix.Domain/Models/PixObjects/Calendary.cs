
using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.PixObjects;
public class Calendary
{
    [JsonPropertyName("criacao")]
    public DateTime Creation { get; set; }

    [JsonPropertyName("expiracao")]
    public string Expiration { get; set; }
}