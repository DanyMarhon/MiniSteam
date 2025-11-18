using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application;
using MiniSteam.Application.Dtos.Licence;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities;
using System.Xml.Linq;

namespace MiniSteam.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]

    public class LicencesController : ControllerBase
    {
        private readonly ILogger<LicencesController> _logger;
        private readonly IApplication<MiniSteam.Entities.Licence> _licence;
        private readonly IMapper _mapper;

        public LicencesController(
            ILogger<LicencesController> logger,
            IApplication<Licence> licence,
            IMapper mapper)
        {
            _logger = logger;
            _licence = licence;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LicenceRequestDto dto)
        {
            if (!ModelState.IsValid)
                throw new MiniSteamException("Validation");
            try
            {
                Licence licence = _mapper.Map<Licence>(dto);
                licence.GenerateKey();
                _licence.Save(licence);
                LicenceResponseDto responseDto = _mapper.Map<LicenceResponseDto>(licence);
                return Ok(responseDto.LicenceKey);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();

            try
            {
                var licence = _licence.GetById(Id.Value);
                if (licence is null)
                    return NotFound();
                int deletedId = licence.Id;
                _licence.Delete(licence.Id);
                return Ok($"Licence {deletedId} successfully deleted");
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
