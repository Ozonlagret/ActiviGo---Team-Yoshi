using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.DTOs.Requests;

namespace Infrastructure.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(int id);
        Task<Activity?> GetByIdWithDetailsAsync(int id); 
        Task<IEnumerable<Activity>> GetAllAsync();
        Task<IEnumerable<Activity>> GetAllActiveAsync();
        Task<IEnumerable<Activity>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Activity>> SearchAsync(string? category, bool? isIndoor);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
