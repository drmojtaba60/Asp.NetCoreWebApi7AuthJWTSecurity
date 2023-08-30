using Asp.NetCore7AuthJwt.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Asp.NetCore7AuthJwt.Services
{
    public static class JwtTokenGenerator
    {
        public static string GenerateToken(UserComplexData userComplex, JwtKeyOptions jwtKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtKey.ValidIssuer,
                Audience = jwtKey.ValidAudience,
                Expires = DateTime.UtcNow.AddSeconds(jwtKey.TokenExpiryTimeInSecond),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512),
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userComplex.Identifier),
                new Claim(ClaimTypes.Name,"JWTTokePracticeInAsp.NetCore7"),
                new Claim(ClaimTypes.Hash, userComplex.ToString()),
            }),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
