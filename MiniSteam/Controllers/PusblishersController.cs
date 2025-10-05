using Microsoft.AspNetCore.Mvc;
using MiniSteam.Application;
using MiniSteam.Entities;

namespace MiniSteam.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisersController : ControllerBase
    {
        private readonly ILogger<PublisersController> _logger;
        private readonly IApplication<Publisher> _publisher;
        public PublisersController(ILogger<PublisersController> logger, IApplication<Publisher> publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_publisher.GetAll());
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
            return Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Publisher publisher)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            _publisher.Save(publisher);
            return Ok(publisher.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int? Id, Publisher publisher)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            Publisher publisherBack = _publisher.GetById(Id.Value);
            if (publisherBack is null)
            { return NotFound(); }
            publisherBack.Name = publisher.Name;
            _publisher.Save(publisherBack);
            return Ok(publisherBack);
        }

        [HttpDelete]
        public async Task<IActionResult> Detele(int? Id)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            Publisher publisherBack = _publisher.GetById(Id.Value);
            if (publisherBack is null)
            { return NotFound(); }
            _publisher.Delete(publisherBack.Id);
            return Ok();
        }
    }
}
