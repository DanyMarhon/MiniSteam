using AutoMapper;
using MiniSteam.Application.Dtos.Identity.Roles;
using MiniSteam.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MiniSteam.WebApi.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<GamesController> _logger;
        private readonly IMapper _mapper;
        public RolesController(RoleManager<Role> roleManager
            , ILogger<GamesController> logger
            , IMapper mapper)
        {
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Obtiene una lista de todos los laboratorios por tipo
        /// </summary>
        /// <returns>Lista de Alimentos</returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IList<RoleResponseDto>>(_roleManager.Roles.ToList()));
        }

        /// <summary>
        /// Crea un alimento
        /// </summary>
        /// <param name="{"></param>
        /// <returns></returns>
        /// <response code="200">Alimento Creado</response>
        /// <response code="400">Error al validar el alimento</response>
        /// <response code="500">Oops! No se pudo crear el alimento</response>
        [HttpPost]
        [Route("Create")]
        public IActionResult Guardar(RoleRequestDto roleRequestDto)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(User.FindFirst("Id")?.Value);
                try
                {
                    var role = _mapper.Map<Role>(roleRequestDto);
                    var result = _roleManager.CreateAsync(role).Result;
                    if (result.Succeeded)
                    {
                        return Ok(role.Id);
                    }
                    return Problem(detail: result.Errors.First().Description, instance: role.Name, statusCode: StatusCodes.Status409Conflict);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                return BadRequest("Invalid data.");
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Modificar([FromBody] RoleRequestDto roleRequestDto, [FromQuery] Guid id)/*Falta */
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(User.FindFirst("Id")?.Value);
                try
                {
                    var role = _mapper.Map<Role>(roleRequestDto);
                    role.Id = id;
                    var result = _roleManager.UpdateAsync(role).Result;
                    if (result.Succeeded)
                    {
                        return Ok(role.Id);
                    }
                    return Problem(detail: result.Errors.First().Description, instance: role.Name, statusCode: StatusCodes.Status409Conflict);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return BadRequest("Invalid data.");
            }
        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return BadRequest();
                }
                var role = _roleManager.FindByIdAsync(id.Value.ToString());
                if (role == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<RoleResponseDto>(role));
            }
            catch (Exception ex)
            {
                return Conflict();
            }
        }
    }
}
