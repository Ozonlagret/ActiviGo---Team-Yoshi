using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repository;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ActiviGoDbContext _context;

        public CategoryRepository(ActiviGoDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
                _context.Entry(category).State = EntityState.Detached;
            return category;
        }

        public async Task<Category?> GetByIdWithActivitiesAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Activities)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
                _context.Categories.Remove(category);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AsNoTracking().AnyAsync(a => a.Id == id);
        }

        public async Task<bool> HasActivitiesAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .AnyAsync(c => c.Id == id && c.Activities.Any());
        }
    }
}
