using AutoMapper;
using MiniSteam.Application.Dtos.Genre;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Mapping
{
    public class GenreMappingProfile : Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<Genre, GenreResponseDto>();
            CreateMap<GenreRequestDto, Genre>();
        }
    }
}
