using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Platform;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ILogger<PlatformsController> _logger;
        private readonly IApplication<Platform> _platform;
        private readonly IMapper _mapper;
        public PlatformsController(ILogger<PlatformsController> logger, IApplication<Platform> platform, IMapper mapper)
        {
            _logger = logger;
            _platform = platform;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            try
            {
                return Ok(_mapper.Map<IList<PlatformResponseDto>>(_platform.GetAll()));
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
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                {
                    return BadRequest();
                }
                Platform platform = _platform.GetById(Id.Value);
                if (platform is null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PlatformResponseDto>(platform));
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
        public async Task<IActionResult> Create(PlatformRequestDto platformRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                { return BadRequest(); }
                var platform = _mapper.Map<Platform>(platformRequestDto);
                _platform.Save(platform);
                return Ok(platform.Id);
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
        public async Task<IActionResult> Edit(int? Id, PlatformRequestDto platformRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                { return BadRequest(); }
                if (!ModelState.IsValid)
                { return BadRequest(); }
                Platform platform = _platform.GetById(Id.Value);
                if (platform is null)
                { return NotFound(); }
                platform = _mapper.Map<Platform>(platformRequestDto);
                _platform.Save(platform);
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

        [HttpDelete]
        public async Task<IActionResult> Detele(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                { return BadRequest(); }
                if (!ModelState.IsValid)
                { return BadRequest(); }
                var platform = _platform.GetById(Id.Value);
                if (platform is null)
                { return NotFound(); }
                _platform.Delete(platform.Id);
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
