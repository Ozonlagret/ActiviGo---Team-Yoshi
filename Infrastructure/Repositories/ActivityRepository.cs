using Infrastructure.Data;
using Domain.Entities;
using Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repository;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ActiviGoDbContext _context;

        public ActivityRepository(ActiviGoDbContext context)
        {
            _context = context;
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Activity?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Sessions)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Sessions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetAllActiveAsync()
        {
            return await _context.Activities
                .Where(a => a.IsActive == true)
                .Include(a => a.Category)
                .Include(a => a.Sessions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Activities
                .Where(a => a.CategoryId == categoryId)
                .Include(a => a.Category)
                .Include(a => a.Sessions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> SearchAsync(string? category, bool? isIndoor)
        {
            var query = _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Sessions)
                    .ThenInclude(s => s.Location)
                .AsQueryable(); 

            if (!string.IsNullOrEmpty(category))
                query = query.Where(a => a.Category.Name == category);

            if (isIndoor.HasValue)
                query = query.Where(a => a.Sessions.Any(s => s.Location.IsIndoor == isIndoor.Value));

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Activity activity, CancellationToken ct = default)
        {
            await _context.Activities.AddAsync(activity, ct);
        }

        public void Update(Activity activity)
        {
            _context.Activities.Update(activity);
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
                _context.Activities.Remove(activity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Activities.AsNoTracking().AnyAsync(a => a.Id == id);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
