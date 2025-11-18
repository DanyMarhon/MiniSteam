using AutoMapper;
using MiniSteam.Application.Dtos.GamerUser;
using MiniSteam.Entities;

public class GamerUserMappingProfile : Profile
{
    public GamerUserMappingProfile()
    {
        CreateMap<GamerUser, GamerUserResponseDto>();
        CreateMap<GamerUserRequestDto, GamerUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ConstructUsing((dto, ctx) =>
            {
                if (dto.Id == 0)
                {
                    return new GamerUser(
                        dto.Name,
                        dto.Email,
                        dto.DateOfBirth,
                        dto.Password!
                    );
                }
                return new GamerUser();
            })
            .AfterMap((src, dest) =>
            {
                dest.SetName(src.Name);
                dest.SetEmail(src.Email);
                dest.SetDateOfBirth(src.DateOfBirth);

                if (!string.IsNullOrWhiteSpace(src.Password))
                    dest.SetPassword(src.Password);
            });
    }
}

