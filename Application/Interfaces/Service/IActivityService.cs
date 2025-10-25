using Application.DTOs.Responses;
using Application.DTOs.Requests;

namespace Application.Interfaces.Service
{
    public interface IActivityService
    {
        Task<IEnumerable<GetActivityDto>> GetAllAsync();
        Task<GetActivityDto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<GetActivityDto>> GetAllActiveAsync();
        Task<IEnumerable<GetActivityDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<GetActivityDto>> SearchAsync(string? category, bool? isIndoor);

        //Admin Operations
        Task<GetActivityDto> CreateAsync(CreateActivityRequest req, CancellationToken ct = default);
        Task<bool> UpdateAsync(UpdateActivityRequest req, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct = default);
    }
}
