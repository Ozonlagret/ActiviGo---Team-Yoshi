using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ActivitySession
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Guid LocationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }         // spots for this occurrence
        public bool IsCancelled { get; set; } = false;

        // Navigation
        public Activity Activity { get; set; } = null!;
        public Location Location { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
