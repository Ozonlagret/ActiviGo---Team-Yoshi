using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ActivitySession
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int LocationId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int Capacity { get; set; }         // spots for this occurrence
        public bool IsCanceled { get; set; } = false;

        // Navigation
        public Activity Activity { get; set; } = null!;
        public Location Location { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
