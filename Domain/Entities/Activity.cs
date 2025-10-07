using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public TimeSpan StandardDuration { get; set; } 
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int CancellationCutoffHours { get; set; } = 24;

        // Navigation
        public Category Category { get; set; } = null!;
        public ICollection<ActivitySession> Sessions { get; set; } = new List<ActivitySession>();
    }
}