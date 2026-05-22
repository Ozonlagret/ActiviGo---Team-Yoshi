using System;

namespace Application.DTOs.Responses
{
    public sealed record HealthLogResponse(
        int Id,
        DateOnly LogDate,
        int? Steps,
        int? Calories,
        decimal? WeightKg,
        int? HeightCm,
        DateTime CreatedAtUtc,
        DateTime UpdatedAtUtc
    );
}