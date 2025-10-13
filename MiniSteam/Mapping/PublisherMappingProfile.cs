using AutoMapper;
using MiniSteam.Application.Dtos.Game;
using MiniSteam.Application.Dtos.Publisher;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Mapping
{
    public class PublisherMappingProfile : Profile
    {
        public PublisherMappingProfile()
        {
            CreateMap<Publisher, PublisherResponseDto>();
            CreateMap<PublisherRequestDto, Publisher>();
        }
    }
}
