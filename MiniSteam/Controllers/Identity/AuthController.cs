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
    [AllowAnonymous]
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
                        var response = _mapper.Map<UserRegistroResponseDto>(user) ?? new UserRegistroResponseDto();

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
        [Route("login")]
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

                var roles = await _userManager.GetRolesAsync(userExists);
                var parameters = new TokenParameters
                {
                    Id = userExists.Id.ToString(),
                    PaswordHash = userExists.PasswordHash,
                    UserName = userExists.UserName,
                    Email = userExists.Email,
                    Roles = roles
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
