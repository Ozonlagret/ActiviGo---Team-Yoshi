using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.DTOs.Responses
{
    public record ActivityResponse
    {
        public int Id { get; init; } 
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public TimeSpan StandardDuration { get; init; }
        public decimal Price { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}
    