﻿using System.Text.Json.Serialization;

namespace Webhook.PayHub.Application.Dtos.ResponseItauWebHook;


public partial class Loot
{
    [JsonPropertyName("valor")]
    public string Amount { get; set; }

    [JsonPropertyName("modalidadAgente")]
    public string AgentModality { get; set; }

    [JsonPropertyName("prestadorDeServicoDeSaque")]
    public string WithdrawalServiceProvider { get; set; }
}