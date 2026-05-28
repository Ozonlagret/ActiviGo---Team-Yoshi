using Application.DTOs.Requests;
using Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActiviGo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/health-tracker")]
    public class HealthTrackerController : ControllerBase
    {
        private readonly IHealthTrackerService _service;

        public HealthTrackerController(IHealthTrackerService service)
        {
            _service = service;
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs(CancellationToken ct)
        {
            var userId = GetUserId();
            if (userId <= 0) return Unauthorized();

            return Ok(await _service.GetHistoryAsync(userId, ct));
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken ct)
        {
            var userId = GetUserId();
            if (userId <= 0) return Unauthorized();

            var profile = await _service.GetProfileAsync(userId, ct);
            return profile is null ? NotFound() : Ok(profile);
        }

        [HttpPost("logs")]
        public async Task<IActionResult> SaveLog([FromBody] SaveHealthLogRequest request, CancellationToken ct)
        {
            var userId = GetUserId();
            if (userId <= 0) return Unauthorized();

            if (request.Steps is null && request.Calories is null && request.WeightKg is null && request.HeightCm is null)
                return BadRequest("Provide at least one value.");

            var result = await _service.SaveAsync(userId, request, ct);
            return Ok(result);
        }

        private int GetUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            return int.TryParse(sub, out var id) ? id : 0;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(CancellationToken ct)
        {
            var userId = GetUserId();
            if (userId <= 0) return Unauthorized();

            var stats = await _service.GetStatsAsync(userId, ct);
            return Ok(stats);
        }
    }
}