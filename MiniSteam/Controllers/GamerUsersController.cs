using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.GamerUser;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamerUsersController : ControllerBase
    {
        private readonly ILogger<GamerUsersController> _logger;
        private readonly IApplication<GamerUser> _gamerUsers;
        private readonly IMapper _mapper;

        public GamerUsersController(
            ILogger<GamerUsersController> logger,
            IApplication<GamerUser> gamerUsers,
            IMapper mapper)
        {
            _logger = logger;
            _gamerUsers = gamerUsers;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            try
            {
                var users = _gamerUsers.GetAll();
                return Ok(_mapper.Map<IList<GamerUserResponseDto>>(users));
            }
            catch (AutoMapperMappingException ex)
            {
                throw new MiniSteamException("Mapping", ex);
            }
            catch (SqlException ex)
            {
                throw new MiniSteamException("Database", ex);
            }
            catch (Exception ex)
            {
                throw new MiniSteamException("Service", ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest();

                GamerUser user = _gamerUsers.GetById(Id.Value);

                if (user is null)
                    return NotFound();

                return Ok(_mapper.Map<GamerUserResponseDto>(user));
            }
            catch (AutoMapperMappingException ex)
            {
                throw new MiniSteamException("Mapping", ex);
            }
            catch (SqlException ex)
            {
                throw new MiniSteamException("Database", ex);
            }
            catch (Exception ex)
            {
                throw new MiniSteamException("Service", ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(GamerUserRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var user = _mapper.Map<GamerUser>(dto);

                _gamerUsers.Save(user);

                return Ok(user.Id);
            }
            catch (AutoMapperMappingException ex)
            {
                throw new MiniSteamException("Mapping", ex);
            }
            catch (SqlException ex)
            {
                throw new MiniSteamException("Database", ex);
            }
            catch (Exception ex)
            {
                throw new MiniSteamException("Service", ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? Id, GamerUserRequestDto dto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest();

                GamerUser user = _gamerUsers.GetById(Id.Value);

                if (user is null)
                    return NotFound();

                dto.Id = Id.Value;
                _mapper.Map(dto, user);

                _gamerUsers.Save(user);

                return Ok();
            }
            catch (AutoMapperMappingException ex)
            {
                throw new MiniSteamException("Mapping", ex);
            }
            catch (SqlException ex)
            {
                throw new MiniSteamException("Database", ex);
            }
            catch (Exception ex)
            {
                throw new MiniSteamException("Service", ex);
            }
        }

    }
}