using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application.Dtos.Identity.Roles;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities.MicrosoftIdentity;

namespace MiniSteam.WebApi.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<RolesController> _logger;
        private readonly IMapper _mapper;
        public RolesController(RoleManager<Role> roleManager
            , ILogger<RolesController> logger
            , IMapper mapper)
        {
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                var dto = _mapper.Map<IList<RoleResponseDto>>(roles);
                return Ok(dto);
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
        [Route("Create")]
        [AllowAnonymous]
        public async Task<IActionResult> Guardar([FromBody] RoleRequestDto roleRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = _mapper.Map<Role>(roleRequestDto);
                role.Id = Guid.NewGuid();
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(role.Id);
                }
                return Problem(detail: result.Errors.FirstOrDefault()?.Description, instance: role.Name, statusCode: StatusCodes.Status409Conflict);
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
        [Route("Update")]
        public async Task<IActionResult> Modificar([FromBody] RoleRequestDto roleRequestDto, [FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingRole = await _roleManager.FindByIdAsync(id.ToString());
                if (existingRole == null)
                {
                    return NotFound();
                }

                existingRole.Name = roleRequestDto.Name;

                var result = await _roleManager.UpdateAsync(existingRole);
                if (result.Succeeded)
                {
                    return Ok(existingRole.Id);
                }

                return Problem(detail: result.Errors.FirstOrDefault()?.Description, instance: existingRole.Name, statusCode: StatusCodes.Status409Conflict);
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

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var role = await _roleManager.FindByIdAsync(id.Value.ToString());
                if (role == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<RoleResponseDto>(role));
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
