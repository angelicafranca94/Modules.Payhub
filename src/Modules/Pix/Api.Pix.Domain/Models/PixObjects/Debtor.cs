using System.Text.Json.Serialization;

namespace Api.Pix.Domain.Models.PixObjects;
public class Debtor
{
    [JsonPropertyName("cnpj")]
    public string Cnpj { get; set; }

    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }

    [JsonPropertyName("nome")]
    public string Name { get; set; }
}