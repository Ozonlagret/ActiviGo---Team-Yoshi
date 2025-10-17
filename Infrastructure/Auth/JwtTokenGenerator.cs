using Application.Interfaces;
using Application.Options;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Auth
{
    public sealed class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _options;
        public JwtTokenGenerator(IOptions<JwtOptions> options) => _options = options.Value;

        public (string token, DateTime expiresAtUtc) Generate(
            int userId,
            string? email,
            string? displayName,
            IEnumerable<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var name = string.IsNullOrWhiteSpace(displayName)
                ? (email ?? userId.ToString())
                : displayName!;

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email ?? string.Empty),
            new(ClaimTypes.Name, name),
            new("sub", userId.ToString())
        };

            if (roles != null)
            {
                foreach (var r in roles)
                    claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var expires = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }
    }
}