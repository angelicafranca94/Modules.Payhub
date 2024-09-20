using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.PixObjects;
public class InformationAdditional
{
    [JsonPropertyName("nome")]
    public string Name { get; set; }

    [JsonPropertyName("valor")]
    public string Amount { get; set; }
}

