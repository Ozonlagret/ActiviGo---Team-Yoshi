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

        public string Name { get; set; }

        public string Email { get; set; }

        // Bcrypt hash
        public string PasswordHash { get; set; } = null!;

        // "User" or "Admin"
        public string Role { get; set; } = "User";
    }
}
