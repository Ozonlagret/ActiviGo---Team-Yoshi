using System;

namespace Application.DTOs.Responses
{
    public sealed record HealthProfileResponse(
        decimal? WeightKg,
        int? HeightCm,
        DateOnly? LogDate,
        DateTime? UpdatedAtUtc
    );
}