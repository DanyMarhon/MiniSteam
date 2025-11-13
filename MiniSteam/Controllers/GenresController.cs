using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Genre;
using MiniSteam.CustomExceptions;
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
            try
            {
                return Ok(_mapper.Map<IList<GenreResponseDto>>(_genre.GetAll()));
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
            {
                return BadRequest();
            }

            try
            {
                Genre genre = _genre.GetById(Id.Value);
                if (genre is null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<GenreResponseDto>(genre));
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
        public async Task<IActionResult> Create(GenreRequestDto genreRequestDto)
        {
            if (!ModelState.IsValid)
                throw new MiniSteamException("Validation");

            try
            {
                var genre = _mapper.Map<Genre>(genreRequestDto);

                // Debería hacer el método asíncrono
                _genre.Save(genre);

                return Ok(genre.Id);
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
        public async Task<IActionResult> Edit(int? Id, GenreRequestDto genreRequestDto)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }

            try
            {
                Genre genre = _genre.GetById(Id.Value);
                if (genre is null)
                { return NotFound(); }

                genre = _mapper.Map<Genre>(genreRequestDto);
                _genre.Save(genre);
                return Ok(_mapper.Map<GenreResponseDto>(genre));
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
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }

            try
            {
                var genre = _genre.GetById(Id.Value);
                if (genre is null)
                { return NotFound(); }
                _genre.Delete(genre.Id);
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
