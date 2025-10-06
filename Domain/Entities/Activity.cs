using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool IsIndoor { get; set; }
        public TimeSpan StandardDuration { get; set; } // default duration
        public decimal Price { get; set; }             // base price
        public string? ImageUrl { get; set; }

        // Navigation
        public Category Category { get; set; } = null!;
        public ICollection<ActivitySession> Sessions { get; set; } = new List<ActivitySession>();
    }
}