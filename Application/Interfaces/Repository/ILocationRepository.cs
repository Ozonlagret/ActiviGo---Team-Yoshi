using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface ILocationRepository
    {
        Task<Location?> GetByIdAsync(int id);
        Task<Location?> GetByIdWithSessionsAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
        Task<IEnumerable<Location>> GetActiveAsync();
        Task<IEnumerable<Location>> GetByTypeAsync(bool isIndoor);
        Task AddAsync(Location location);
        void Update(Location location);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
