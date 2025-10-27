using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using Application.DTOs.Responses;
using Application.DTOs.Requests;
using AutoMapper;

namespace Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityResponse>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ActivityResponse>>(activities);
        }

        public async Task<ActivityResponse?> GetByIdAsync(int id)
        {
            var activity = await _repository.GetByIdAsync(id);
            return activity == null ? null : _mapper.Map<ActivityResponse>(activity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<ActivityResponse>> GetAllActiveAsync()
        {
            var activities = await _repository.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<ActivityResponse>>(activities);
        }

        public async Task<IEnumerable<ActivityResponse>> GetByCategoryAsync(int categoryId)
        {
            var activities = await _repository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ActivityResponse>>(activities);
        }

        public async Task<IEnumerable<ActivityResponse>> SearchAsync(string? category, bool? isIndoor)
        {
            var activities = await _repository.SearchAsync(category, isIndoor);
            return _mapper.Map<IEnumerable<ActivityResponse>>(activities);
        }

        public async Task<ActivityResponse> CreateAsync(CreateActivityRequest req, CancellationToken ct)
        {
            var entity = new Activity
            {
                Id = req.Id,
                Name = req.Name ?? string.Empty,
                Description = req.Description,
                CategoryId = req.CategoryId,
                Price = req.Price,
                StandardDuration = req.StandardDuration,
                IsOutdoor = req.IsOutdoor,
                IsActive = req.IsActive,
                ImageUrl = req.ImageUrl,
            };

            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);
            

            return _mapper.Map<ActivityResponse>(entity);
        }
    }
}
