using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Platform;
using MiniSteam.Entities;
using System.Xml.Linq;

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
            return Ok(_mapper.Map<IList<PlatformResponseDto>>(_platform.GetAll()));
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
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

        [HttpPost]
        public async Task<IActionResult> Create(PlatformRequestDto platformRequestDto)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var platform = _mapper.Map<Platform>(platformRequestDto);
            _platform.Save(platform);
            return Ok(platform.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int? Id, PlatformRequestDto platformRequestDto)
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

        [HttpDelete]
        public async Task<IActionResult> Detele(int? Id)
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
    }
}
