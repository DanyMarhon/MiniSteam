using AutoMapper;
using MiniSteam.Application.Dtos.Game;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Mapping
{
    public class GameMappingProfile : Profile
    {
        public GameMappingProfile()
        {
            CreateMap<Game, GameResponseDto>().
                ForMember(dest => dest.ReleaseDate, ori => ori.MapFrom(src => src.ReleaseDate.ToShortDateString()));
            CreateMap<GameRequestDto, Game>();
        }
    }

}
