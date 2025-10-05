namespace ActiviGo.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAtUtc { get; set; }
        public string Role { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int UserId { get; set; }
    }
}