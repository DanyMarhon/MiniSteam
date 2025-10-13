using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSteam.Application;
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
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_game.GetAll());
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }
            Game game = _game.GetById(Id.Value);
            if (game is null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        //TODO: Implement Create, Edit, Delete
    }
}
