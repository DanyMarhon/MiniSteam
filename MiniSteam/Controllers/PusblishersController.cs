using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Publisher;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly ILogger<PublishersController> _logger;
        private readonly IApplication<Publisher> _publisher;
        private readonly IMapper _mapper;
        public PublishersController(ILogger<PublishersController> logger, IApplication<Publisher> publisher, IMapper mapper)
        {
            _logger = logger;
            _publisher = publisher;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_mapper.Map<IList<PublisherResponseDto>>(_publisher.GetAll()));
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }
            Publisher publisher = _publisher.GetById(Id.Value);
            if (publisher is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PublisherResponseDto>(publisher));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PublisherRequestDto publisherRequestDto)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var publisher = _mapper.Map<Publisher>(publisherRequestDto);
            _publisher.Save(publisher);
            return Ok(publisher.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int? Id, PublisherRequestDto publisherRequestDto)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            Publisher publisher = _publisher.GetById(Id.Value);
            if (publisher is null)
            { return NotFound(); }
            publisher = _mapper.Map<Publisher>(publisherRequestDto);
            _publisher.Save(publisher);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Detele(int? Id)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var publisher = _publisher.GetById(Id.Value);
            if (publisher is null)
            { return NotFound(); }
            _publisher.Delete(publisher.Id);
            return Ok();
        }
    }
}
