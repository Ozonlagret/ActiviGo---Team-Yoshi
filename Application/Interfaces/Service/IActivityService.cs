using Application.DTOs.Requests;
using Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityResponse>> GetAllAsync();
        Task<ActivityResponse?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<ActivityResponse>> GetAllActiveAsync();
        Task<IEnumerable<ActivityResponse>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ActivityResponse>> SearchAsync(string? category, bool? isIndoor);
        Task<ActivityResponse> CreateAsync(CreateActivityRequest req, CancellationToken ct = default);
    }
}
