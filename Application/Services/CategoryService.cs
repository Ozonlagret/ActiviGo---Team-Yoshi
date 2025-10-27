using System;
using Application.Interfaces.Service;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repository;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public async Task<IEnumerable<CategoriesForFilterResponse>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoriesForFilterResponse { Id = c.Id, Name = c.Name });
        }
    }

}