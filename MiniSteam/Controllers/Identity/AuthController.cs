using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.Application.Dtos.Identity.User;
using MiniSteam.Application.Dtos.Login;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities.MicrosoftIdentity;
using MiniSteam.Services.AuthServices;
using MiniSteam.WebApi.Configurations;

namespace MiniSteam.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenHandlerService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(
            UserManager<User> userManager
            , ILogger<AuthController> logger
            , ITokenHandlerService tokenService
            , IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegistroRequestDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExists = await _userManager.FindByEmailAsync(user.Email);
                    if (userExists != null)
                    {
                        return BadRequest("User already exists with mail: " + user.Email + ".");
                    }

                    var newUser = _mapper.Map<User>(user) ?? new User();

                    newUser.Email = user.Email;
                    if (string.IsNullOrWhiteSpace(newUser.UserName) && user.Email.Contains("@"))
                    {
                        newUser.UserName = user.Email.Substring(0, user.Email.IndexOf('@'));
                    }

                    var created = await _userManager.CreateAsync(newUser, user.Password);
                    if (created.Succeeded)
                    {
                        var response = _mapper.Map<UserRegistroResponseDto>(newUser) ?? new UserRegistroResponseDto();
                        response.FullName = string.IsNullOrWhiteSpace(response.FullName) ? string.Join(" ", user.Names, user.Surname) : response.FullName;
                        response.Email = string.IsNullOrWhiteSpace(response.Email) ? newUser.Email : response.Email;
                        response.UserName = string.IsNullOrWhiteSpace(response.UserName) ? newUser.UserName : response.UserName;

                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(created.Errors.Select(e => e.Description).ToList());
                    }
                }
                else
                {
                    return BadRequest("Invalid data");
                }
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
        [Route("RegisterSincronico")]
        public IActionResult RegistrarUsuarioincronico([FromBody] UserRegistroRequestDto user)
        {
            try
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
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto userlogin)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userExists = await _userManager.FindByEmailAsync(userlogin.Email);
                if (userExists == null)
                    return Unauthorized(new LoginUserResponseDto { Login = false, Errors = new System.Collections.Generic.List<string> { "User or Pass wrong!" } });

                var isCorrect = await _userManager.CheckPasswordAsync(userExists, userlogin.Password);
                if (!isCorrect)
                    return Unauthorized(new LoginUserResponseDto { Login = false, Errors = new System.Collections.Generic.List<string> { "User or Pass wrong!" } });

                var parameters = new TokenParameters
                {
                    Id = userExists.Id.ToString(),
                    UserName = userExists.UserName,
                    Email = userExists.Email
                };

                var jwt = _tokenService.GenerateJwtTokens(parameters);
                return Ok(new LoginUserResponseDto { Login = true, Token = jwt, UserName = userExists.UserName, Mail = userExists.Email });
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
