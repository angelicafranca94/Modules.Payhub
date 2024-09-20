using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.PixObjects;
public class Loc
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("tipoCob")]
    public string TipoCob { get; set; }

    [JsonPropertyName("criacao")]
    public DateTime Criacao { get; set; }
}
