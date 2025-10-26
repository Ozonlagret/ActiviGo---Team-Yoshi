namespace Application.DTOs.Requests
{
    public class CreateActivityRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public TimeSpan StandardDuration { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsOutdoor { get; set; }
    }
}