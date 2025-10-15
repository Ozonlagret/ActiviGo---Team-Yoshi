using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public sealed class AuthResponse
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public string Role { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int UserId { get; set; }
    }
}
