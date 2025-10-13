using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Genre;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> _logger;
        private readonly IApplication<Genre> _genre;
        private readonly IMapper _mapper;
        public GenresController(ILogger<GenresController> logger, IApplication<Genre> genre, IMapper mapper)
        {
            _logger = logger;
            _genre = genre;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_genre.GetAll());
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }
            Genre genre = _genre.GetById(Id.Value);
            if (genre is null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenreRequestDto genreRequestDto)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var genre = _mapper.Map<Genre>(genreRequestDto);
            _genre.Save(genre);
            return Ok(genre.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int? Id, GenreRequestDto genreRequestDto)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            Genre genre = _genre.GetById(Id.Value);
            if (genre is null)
            { return NotFound(); }
            genre = _mapper.Map<Genre>(genreRequestDto);
            _genre.Save(genre);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Detele(int? Id)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var genre = _genre.GetById(Id.Value);
            if (genre is null)
            { return NotFound(); }
            _genre.Delete(genre.Id);
            return Ok();
        }
    }
}
