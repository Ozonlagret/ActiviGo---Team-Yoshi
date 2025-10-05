using Application.Interfaces;
using ActiviGo.Requests;
using ActiviGo.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await _auth.LoginAsync(request.Email, request.Password, ct);
            if (result is null) return Unauthorized();

            return Ok(new AuthResponse
            {
                Token = result.Token,
                ExpiresAtUtc = result.ExpiresAtUtc,
                Role = result.Role,
                Name = result.Name,
                UserId = result.UserId
            });
        }
    }
}