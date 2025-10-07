using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(IUserRepository users, IPasswordHasher hasher, IJwtTokenGenerator jwt)
        {
            _users = users;
            _hasher = hasher;
            _jwt = jwt;
        }

        public async Task<AuthResult?> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _users.GetByEmailAsync(email, ct);
            if (user is null) return null;
            if (!_hasher.Verify(password, user.PasswordHash)) return null;

            var (token, expiresAtUtc) = _jwt.Generate(user);

            return new AuthResult
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                Role = user.Role,
                Name = user.UserName,
                UserId = user.Id
            };
        }
    }
}