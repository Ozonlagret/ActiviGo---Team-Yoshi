using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ActiviGoDbContext _context;

        public LocationRepository(ActiviGoDbContext context)
        {
            _context = context;
        }

        // CHANGES ARE TO BE SAVED IN SERVICE LAYER

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task<Location?> GetByIdWithSessionsAsync(int id)
        {
            return await _context.Locations
                .Include(l => l.Sessions)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _context.Locations
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetActiveAsync()
        {
            return await _context.Locations
                .Where(l => l.IsActive == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetByTypeAsync(bool isIndoor)
        {
            if (isIndoor == true)
            {
                return await _context.Locations
                    .Where(l => l.IsIndoor)
                    .ToListAsync();
            }
            else
            {
                return await _context.Locations
                    .Where(l => !l.IsIndoor)
                    .ToListAsync();
            }
        }

        public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        public void Update(Location location)
        {
            _context.Locations.Update(location);
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
                _context.Locations.Remove(location);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Locations.AnyAsync(a => a.Id == id);
        }
    }
}
