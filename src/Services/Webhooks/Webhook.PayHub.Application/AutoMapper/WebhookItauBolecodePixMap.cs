using AutoMapper;
using Webhook.PayHub.Application.Dtos.ResponseItauWebHook;
using Webhook.PayHub.Domain.Models;
using Webhook.PayHub.Domain.Models.ResponseItauWebHook;

namespace Webhook.PayHub.Application.AutoMapper;

public class WebhookItauBolecodePixMap : Profile
{
    public WebhookItauBolecodePixMap()
    {

        CreateMap<ItauWebhookDto, ItauWebhookModel>();
        CreateMap<WebhookItauBolecodePixDto, WebhookItauBolecodePixModel>();


        CreateMap<ComponentsAmount, ComponentsAmountModel>();
        CreateMap<Original, OriginalModel>();
        CreateMap<Loot, LootModel>();
        CreateMap<Change, ChangeModel>();
        CreateMap<Fees, FeesModel>();
        CreateMap<Fine, FineModel>();
        CreateMap<Reduction, ReductionModel>();
        CreateMap<Discount, DiscountModel>();

        CreateMap<Devolution, DevolutionModel>();
        CreateMap<Time, TimeModel>();
    }
}
