using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActivityOccurrenceId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; } = BookingStatus.Active;

        // Navigation
        public User User { get; set; } = null!;
        public ActivitySession ActivitySession { get; set; } = null!;
    }
}
