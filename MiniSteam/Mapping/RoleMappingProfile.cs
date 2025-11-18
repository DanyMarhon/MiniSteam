using AutoMapper;
using MiniSteam.Application.Dtos.Identity.Roles;
using MiniSteam.Entities.MicrosoftIdentity;

namespace MiniSteam.WebApi.Mapping
{
    public class RoleMappingProfile: Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleResponseDto>();
            CreateMap<RoleRequestDto, Role>();
        }
    }
}
