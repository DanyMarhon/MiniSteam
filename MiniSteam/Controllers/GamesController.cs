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

        public GamesController(
            ILogger<GamesController> logger,
            IApplication<Game> game,
            IMapper mapper)
        {
            _logger = logger;
            _game = game;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            try
            {
                var list = _game.GetAll();
                return Ok(_mapper.Map<IList<GameResponseDto>>(list));
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
            if (!Id.HasValue)
                return BadRequest();

            try
            {
                var game = _game.GetById(Id.Value);
                if (game is null)
                    return NotFound();

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
        public async Task<IActionResult> Create(GameRequestDto dto)
        {
            if (!ModelState.IsValid)
                throw new MiniSteamException("Validation");

            try
            {
                Game game = _mapper.Map<Game>(dto);
                _game.Save(game);

                return Ok(game.Id);
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
        public async Task<IActionResult> Edit([FromBody] GameRequestDto dto)
        {
            if (dto is null || dto.Id == 0)
                return BadRequest("El DTO es inválido.");

            try
            {
                var existing = _game.GetById(dto.Id);
                if (existing is null)
                    return NotFound("El juego no existe.");

                _mapper.Map(dto, existing);

                var saved = _game.Save(existing);

                var response = _mapper.Map<GameResponseDto>(saved);

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new MiniSteamException("GameService.Edit", ex);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();

            try
            {
                var game = _game.GetById(Id.Value);
                if (game is null)
                    return NotFound();

                _game.Delete(game.Id);
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
