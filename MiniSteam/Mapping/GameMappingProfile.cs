using AutoMapper;
using MiniSteam.Application.Dtos.Game;
using MiniSteam.Entities;

public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<Game, GameResponseDto>()
            .ForMember(dest => dest.GameClassification,
                opt => opt.MapFrom(src => src.Classification.ToString()));

        CreateMap<GameRequestDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ConstructUsing((dto, ctx) =>
            {
                return new Game(
                    dto.Title,
                    dto.IdPublisher,
                    dto.ReleaseDate,
                    dto.Price,
                    dto.CoverImageUrl,
                    dto.Classification
                );
            })
            .AfterMap((src, dest) =>
            {
                dest.SetTitle(src.Title);
                dest.SetDescription(src.Description);
                dest.SetPrice(src.Price);
                dest.SetReleaseDate(src.ReleaseDate);
                dest.SetCoverImageUrl(src.CoverImageUrl);
                dest.SetClassification(src.Classification);
                dest.SetIdPublisher(src.IdPublisher);
            });
    }
}