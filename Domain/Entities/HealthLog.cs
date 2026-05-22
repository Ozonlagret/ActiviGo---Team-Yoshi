using System;

namespace Domain.Entities
{
    public class HealthLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly LogDate { get; set; }
        public int? Steps { get; set; }
        public int? Calories { get; set; }
        public decimal? WeightKg { get; set; }
        public int? HeightCm { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;
    }
}