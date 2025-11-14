using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSteam.CustomExceptions;
using MiniSteam.Entities.MicrosoftIdentity;

namespace MiniSteam.WebApi.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;
        public UsersController(RoleManager<Role> roleManager
            , UserManager<User> userManage
            , ILogger<UsersController> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManage;
        }


        [HttpPost]
        [Route("AddRoleToUser")]
        public async Task<IActionResult> Guardar(string userId, string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId))
            {
                return BadRequest(new { error = "Both userId and roleId are required." });
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    _logger.LogInformation("User not found. userId: {UserId}", userId);
                    return NotFound(new { userId });
                }

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role is null)
                {
                    _logger.LogInformation("Role not found. roleId: {RoleId}", roleId);
                    return NotFound(new { roleId });
                }

                var status = await _userManager.AddToRoleAsync(user, role.Name);
                if (status.Succeeded)
                {
                    return Ok(new { user = user.UserName, role = role.Name });
                }

                var errors = status.Errors?.Select(e => e.Description).ToArray() ?? Array.Empty<string>();
                _logger.LogWarning("Failed to add role '{Role}' to user '{User}'. Errors: {Errors}", role.Name, user.UserName, string.Join("; ", errors));
                return BadRequest(new { errors });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping exception adding role '{RoleId}' to user '{UserId}'", roleId, userId);
                throw new MiniSteamException("Mapping", ex);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database exception adding role '{RoleId}' to user '{UserId}'", roleId, userId);
                throw new MiniSteamException("Database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception adding role '{RoleId}' to user '{UserId}'", roleId, userId);
                throw new MiniSteamException("Service", ex);
            }
        }
    }
}
