using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string token, DateTime expiresAtUtc) Generate(
            int userId,
            string? email,
            string? displayName,
            IEnumerable<string> roles);
    }
}