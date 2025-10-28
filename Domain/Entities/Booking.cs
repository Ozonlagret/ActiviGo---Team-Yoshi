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
        
        // FK till AspNetUsers (IdentityUsers<int>)
        public int UserId { get; set; }
        public int ActivitySessionId { get; set; }
        public DateTime BookingTimeUtc { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; } = BookingStatus.Active;

        public ActivitySession ActivitySession { get; set; } = null!;
    }
}
