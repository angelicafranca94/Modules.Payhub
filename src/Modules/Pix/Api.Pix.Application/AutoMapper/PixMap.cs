using Api.Pix.Application.Dtos;
using Api.Pix.Domain.Models;
using Api.Pix.Domain.Models.Responses;
using AutoMapper;


namespace Api.Pix.Application.AutoMapper;

public class PixMap : Profile
{
    public PixMap()
    {
        CreateMap<PixResponse, PixDto>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Original))
            .ForMember(dest => dest.Creation, opt => opt.MapFrom(src => src.Calendary.Creation))
            .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => Convert.ToInt32(src.Calendary.Expiration)));

        CreateMap<PixControlModel, PixDto>()
            .ForMember(dest => dest.Creation, opt => opt.MapFrom(src => src.DateTransaction))
            .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.ExpirationTime));
    }
}
