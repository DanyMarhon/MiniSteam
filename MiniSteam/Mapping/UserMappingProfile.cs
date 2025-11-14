using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using MiniSteam.Application.Dtos.Genre;
using MiniSteam.Application.Dtos.Identity.User;
using MiniSteam.Application.Dtos.Login;
using MiniSteam.Entities;
using MiniSteam.Entities.MicrosoftIdentity;

namespace MiniSteam.WebApi.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserRegistroRequestDto>();
            CreateMap<UserRegistroRequestDto, User>();

            CreateMap<UserRegistroRequestDto, UserRegistroResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Names} {src.Surname}"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.Names} {src.Surname}"))
                .ReverseMap();
        }
    }
}
