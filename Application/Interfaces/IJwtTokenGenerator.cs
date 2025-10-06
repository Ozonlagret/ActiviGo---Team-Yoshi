using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string token, DateTime expiresAtUtc) Generate(User user);
    }
}