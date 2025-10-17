using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Options;
using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IIdentityAuthRepository _repo;
        private readonly IJwtTokenGenerator _jwt;

        private const string DefaultRole = "Member";

        public AuthService(IIdentityAuthRepository repo, IJwtTokenGenerator jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _repo.FindByEmailAsync(email, ct);
            if (user is null || !user.IsActive) return null;

            var ok = await _repo.CheckPasswordAsync(user, password, ct);
            if (!ok) return null;

            var roles = await _repo.GetRolesAsync(user, ct);

            var displayName = string.Join(' ', new[] { user.FirstName, user.LastName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = user.UserName ?? user.Email ?? $"user-{user.Id}";

            var (token, expiresAtUtc) = _jwt.Generate(user.Id, user.Email, displayName, roles);

            return new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                Role = roles.FirstOrDefault(),
                Name = displayName,
                UserId = user.Id
            };
        }

        public async Task<(bool ok, AuthResponse? resp, IEnumerable<string> errors)> RegisterAsync(
            RegisterRequest req, CancellationToken ct = default)
        {
            if (await _repo.EmailExistsAsync(req.Email, ct))
                return (false, null, new[] { "Emailen används redan." });

            var user = new ApplicationUser
            {
                Email = req.Email,
                UserName = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName,
                EmailConfirmed = true,
                IsActive = true
            };

            var (created, errs) = await _repo.CreateAsync(user, req.Password, ct);
            if (!created) return (false, null, errs);

            var role = string.IsNullOrWhiteSpace(req.Role) ? DefaultRole : req.Role!;
            await _repo.EnsureRoleExistsAsync(role, ct);
            await _repo.AddToRoleAsync(user, role, ct);

            var roles = await _repo.GetRolesAsync(user, ct);

            var displayName = string.Join(' ', new[] { user.FirstName, user.LastName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = user.UserName ?? user.Email ?? $"user-{user.Id}";

            var (token, expiresAtUtc) = _jwt.Generate(user.Id, user.Email, displayName, roles);

            var resp = new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                Role = roles.FirstOrDefault(),
                Name = displayName,
                UserId = user.Id
            };

            return (true, resp, Array.Empty<string>());
        }
    }
}