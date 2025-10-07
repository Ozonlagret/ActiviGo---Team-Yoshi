using Domain.Entities.Enums;

namespace Application.DTOs
{
    public class AuthResult
    {
        public string Token { get; init; } = null!;
        public DateTime ExpiresAtUtc { get; init; }
        public Role Role { get; init; } 
        public string Name { get; init; } = null!;
        public int UserId { get; init; }
    }
}