namespace Application.DTOs
{
    public class AuthResult
    {
        public string Token { get; init; } = null!;
        public DateTime ExpiresAtUtc { get; init; }
        public string Role { get; init; } = null!;
        public string Name { get; init; } = null!;
        public int UserId { get; init; }
    }
}