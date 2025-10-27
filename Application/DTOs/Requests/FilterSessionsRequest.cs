using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public class FilterSessionsRequest
    {
        [JsonPropertyName("dateFrom")]
        public DateTime? StartDate { get; set; }
        
        [JsonPropertyName("dateTo")]
        public DateTime? EndDate { get; set; }
        
        public int? CategoryId { get; set; }
        public bool? IsIndoor { get; set; }
        public int? LocationId { get; set; }
    }
}
