using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Service;

namespace ActiviGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IActivityService _activities;

        public AdminController(IActivityService activities)
        {
            _activities = activities;
        }

        [HttpPost("activities")]
        public async Task<ActionResult<GetActivityDto>> Create([FromBody] CreateActivityRequest req, CancellationToken ct)
        {
            var created = await _activities.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("activities/{id:int}")]
        public async Task<ActionResult<GetActivityDto>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var item = await _activities.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPut("activities/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateActivityRequest req, CancellationToken ct)
        {
            if (id != req.Id) return BadRequest("Mismatching ids.");
            var ok = await _activities.UpdateAsync(req, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("activities/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _activities.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPut("activities/{id:int}/activate")]
        public async Task<IActionResult> Activate([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _activities.SetActiveAsync(id, true, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPut("activities/{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _activities.SetActiveAsync(id, false, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
