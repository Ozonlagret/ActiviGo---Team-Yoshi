using System;

namespace Application.DTOs.Requests
{
    public sealed record SaveHealthLogRequest
    {
        public DateOnly? LogDate { get; init; }
        public int? Steps { get; init; }
        public int? Calories { get; init; }
        public decimal? WeightKg { get; init; }
        public int? HeightCm { get; init; }
    }
}