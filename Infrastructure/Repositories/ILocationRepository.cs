using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ILocationRepository
    {
        Task<Location?> GetByIdAsync(int id);
        Task<Location?> GetByIdWithSessionsAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
        Task<IEnumerable<Location>> GetActiveAsync();
        Task<IEnumerable<Location>> GetByTypeAsync(bool isIndoor);
        Task AddAsync(Location location);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
