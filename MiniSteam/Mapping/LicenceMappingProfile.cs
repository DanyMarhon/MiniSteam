using AutoMapper;
using MiniSteam.Application.Dtos.Licence;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Mapping
{
    public class LicenceMappingProfile : Profile
    {
        public LicenceMappingProfile()
        {
            CreateMap<Licence, LicenceResponseDto>().ReverseMap();
            CreateMap<LicenceRequestDto, Licence>().ReverseMap();
        }
    }
}
