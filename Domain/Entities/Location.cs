using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsIndoor { get; set; }
        public int Capacity { get; set; }       // total spots
        public bool IsActive { get; set; }

        public ICollection<ActivitySession> Sessions { get; set; } = new List<ActivitySession>();
    }
}
