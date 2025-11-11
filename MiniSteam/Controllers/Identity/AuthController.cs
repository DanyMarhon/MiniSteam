using MiniSteam.Application.Dtos.Identity.User;
using MiniSteam.Application.Dtos.Login;
using MiniSteam.Entities.MicrosoftIdentity;
using MiniSteam.Services.AuthServices;
using MiniSteam.WebApi.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MiniSteam.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GamesController> _logger;
        private readonly ITokenHandlerService _servicioToken;
        public AuthController(
            UserManager<User> userManager
            , ILogger<GamesController> logger
            , ITokenHandlerService servicioToken)
        {
            _userManager = userManager;
            _logger = logger;
            _servicioToken = servicioToken;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UserRegistroRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var existeUsuario = await _userManager.FindByEmailAsync(user.Email);
                if (existeUsuario != null)
                {
                    return BadRequest("Existe un usuario registrado con el mal " + user.Email + ".");
                }
                var Creado = await _userManager.CreateAsync(new User()
                {
                    Email = user.Email,
                    UserName = user.Email.Substring(0, user.Email.IndexOf('@')),
                    Names = user.Names,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate
                }, user.Password);
                if (Creado.Succeeded)
                {                    
                    return Ok(new UserRegistroResponseDto
                    {
                        FullName = string.Join(" ", user.Names, user.Surname),
                        Email = user.Email,
                        UserName = user.Email.Substring(0, user.Email.IndexOf('@'))
                    });
                }
                
                else
                {
                    return BadRequest(Creado.Errors.Select(e => e.Description).ToList());
                }
            }
            else
            {
                return BadRequest("Invalid data");
            }
        }

        [HttpPost]
        [Route("RegisterSincronico")]
        public IActionResult RegistrarUsuarioincronico([FromBody] UserRegistroRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var existeUsuario = _userManager.FindByEmailAsync(user.Email).Result;
                if (existeUsuario != null)
                {
                    return BadRequest("Mail " + user.Email + " already in use.");
                }
                var Creado = _userManager.CreateAsync(new User()
                {
                    Email = user.Email,
                    UserName = user.Email.Substring(0, user.Email.IndexOf('@')),
                    Names = user.Names,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate
                }, user.Password).Result;
                if (Creado.Succeeded)
                {
                    var userBack = _userManager.FindByEmailAsync(user.Email);
                    _ = _userManager.AddToRoleAsync(userBack.Result, "Administrator");
                    return Ok(new UserRegistroResponseDto
                    {
                        FullName = string.Join(" ", user.Names, user.Surname),
                        Email = user.Email,
                        UserName = user.Email.Substring(0, user.Email.IndexOf('@'))
                    });
                }
                else
                {
                    return BadRequest(Creado.Errors.Select(e => e.Description).ToList());
                }
            }
            else
            {
                return BadRequest("Invalid data.");
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto userlogin)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(userlogin.Email);
                if (userExists != null)
                {
                    var isCorrect = await _userManager.CheckPasswordAsync(userExists, userlogin.Password);
                    if (isCorrect)
                    {
                        try
                    {
                        var parameters = new TokenParameters()
                        {
                            Id = userExists.Id.ToString(),
                            PaswordHash = userExists.PasswordHash,
                            UserName = userExists.UserName,
                            Email = userExists.Email
                        };
                        var jwt = _servicioToken.GenerateJwtTokens(parameters);
                        return Ok(new LoginUserResponseDto()
                        {
                            Login = true,
                            Token = jwt,
                            UserName = userExists.UserName,
                            Mail = userExists.Email
                        });
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                }
            }
            return BadRequest(new LoginUserResponseDto()
            {
                Login = false,
                Errors = new List<string>()
                    {
                       "User or Pass wrong!"
                    }
            });
        }
    }
}
