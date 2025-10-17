using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(string email, string password, CancellationToken ct = default);
        Task<(bool ok, AuthResponse? resp, IEnumerable<string> errors)> RegisterAsync(RegisterRequest req, CancellationToken ct = default);
    }
}