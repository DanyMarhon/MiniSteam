using AutoMapper;
using MiniSteam.Application.Dtos.Platform;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Mapping
{
    public class PlatformMappingProfile : Profile
    {
        public PlatformMappingProfile()
        {
            CreateMap<Platform, PlatformResponseDto>();
            CreateMap<PlatformRequestDto, Platform>();
        }
    }
}
