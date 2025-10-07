using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Role Role { get; set; } 

        // Bcrypt hash
        public string PasswordHash { get; set; } = null!;

        // "User" or "Admin"
        //public string Role { get; set; } = "User";

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
