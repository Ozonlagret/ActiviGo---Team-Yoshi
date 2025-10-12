using Application.DTOs.Requests;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
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
        void Update(Activity activity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
