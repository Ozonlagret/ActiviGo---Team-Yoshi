using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class FilterActivitySessionResponse
    {
        public int Id { get; set; }
        public string? ActivityName { get; set; }
        public string? LocationName { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int Capacity { get; set; }
        public bool IsCanceled { get; set; }
        public string? ImageUrl { get; set; }
    }
}
