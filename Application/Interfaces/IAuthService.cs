namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<DTOs.AuthResult?> LoginAsync(string email, string password, CancellationToken ct = default);
    }
}