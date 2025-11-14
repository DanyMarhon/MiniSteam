using MiniSteam.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniSteam.Services.AuthServices
{
    public interface ITokenHandlerService
    {
        string GenerateJwtTokens(ITokensParameters parameters);
    }
    public class TokenHandlerService : ITokenHandlerService
    {
        private readonly JwtConfig _jwtConfig;
        public TokenHandlerService(IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        public string GenerateJwtTokens(ITokensParameters parameters)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var claims = new List<Claim>
            {
                new Claim("Id", parameters.Id),
                new Claim(JwtRegisteredClaimNames.Sub, parameters.Id),
                new Claim(JwtRegisteredClaimNames.Name, parameters.UserName),
                new Claim(JwtRegisteredClaimNames.Email, parameters.Email)
            };
            if (parameters.Roles != null && parameters.Roles.Any())
            {
                foreach (var role in parameters.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
