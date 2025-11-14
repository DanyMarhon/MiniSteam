using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Game;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly IApplication<Game> _game;
        private readonly IMapper _mapper;
        public GamesController(ILogger<GamesController> logger, IApplication<Game> game, IMapper mapper)
        {
            _logger = logger;
            _game = game;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            try
            {
                return Ok(_mapper.Map<IList<GameResponseDto>>(_game.GetAll()));
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
        [AllowAnonymous]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                Game game = _game.GetById(Id.Value);
                if (game is null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<GameResponseDto>(game));
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
        [Authorize(Roles = "Admin, ExtendedUser")]
        public async Task<IActionResult> Create([FromBody] Game game)
        {
            if (game is null)
            {
                return BadRequest();
            }

            try
            {
                var saved = _game.Save(game);
                var dto = _mapper.Map<GameResponseDto>(saved);
                return CreatedAtAction(nameof(ById), new { Id = saved.Id }, dto);
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
        [Route("Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromBody] Game game)
        {
            if (game is null || game.Id == 0)
            {
                return BadRequest();
            }

            try
            {
                var existing = _game.GetById(game.Id);
                if (existing is null)
                {
                    return NotFound();
                }

                var saved = _game.Save(game);
                return Ok(_mapper.Map<GameResponseDto>(saved));
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
        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var existing = _game.GetById(Id.Value);
                if (existing is null)
                {
                    return NotFound();
                }

                _game.Delete(Id.Value);
                return NoContent();
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
