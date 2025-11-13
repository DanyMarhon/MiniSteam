using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Publisher;
using MiniSteam.CustomExceptions;
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
            try
            {
                var list = _publisher.GetAll();
                return Ok(_mapper.Map<IList<PublisherResponseDto>>(list));
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
                Publisher publisher = _publisher.GetById(Id.Value);
                if (publisher is null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PublisherResponseDto>(publisher));
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
        public async Task<IActionResult> Create(PublisherRequestDto publisherRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                { return BadRequest(); }
                var publisher = _mapper.Map<Publisher>(publisherRequestDto);
                _publisher.Save(publisher);
                return Ok(publisher.Id);
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
        public async Task<IActionResult> Edit(int? Id, PublisherRequestDto publisherRequestDto)
        {
            try
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
                var publisher = _publisher.GetById(Id.Value);
                if (publisher is null)
                { return NotFound(); }
                _publisher.Delete(publisher.Id);
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
