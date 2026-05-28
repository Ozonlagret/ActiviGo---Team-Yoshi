namespace Application.DTOs.Responses
{
    public sealed record HealthStatsResponse(
        int TotalEntries,
        int TotalSteps,
        int TotalCalories,
        decimal? AverageWeightKg
    );
}