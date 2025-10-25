using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
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

        public async Task<IEnumerable<GetActivityDto>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetActivityDto>>(activities);
        }

        public async Task<GetActivityDto> GetByIdAsync(int id)
        {
            var activity = await _repository.GetByIdAsync(id);
            return activity == null ? null : _mapper.Map<GetActivityDto>(activity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<IEnumerable<GetActivityDto>> GetAllActiveAsync()
        {
            var activities = await _repository.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<GetActivityDto>>(activities);
        }

        public async Task<IEnumerable<GetActivityDto>> GetByCategoryAsync(int categoryId)
        {
            var activities = await _repository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<GetActivityDto>>(activities);
        }

        public async Task<IEnumerable<GetActivityDto>> SearchAsync(string? category, bool? isIndoor)
        {
            var activities = await _repository.SearchAsync(category, isIndoor);
            return _mapper.Map<IEnumerable<GetActivityDto>>(activities);
        }

        //Admin Operations
        public async Task<GetActivityDto> CreateAsync(CreateActivityRequest req, CancellationToken ct = default)
        {
            var entity = new Domain.Entities.Activity
            {
                Name = req.Name,
                Description = req.Description,
                CategoryId = req.CategoryId,
                StandardDuration = req.StandardDuration,
                Price = req.Price,
                ImageUrl = req.ImageUrl,
                IsOutdoor = req.IsOutdoor,
                IsActive = true
            };

            await _repository.AddAsync(entity);
            return _mapper.Map<GetActivityDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateActivityRequest req, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(req.Id);
            if (entity is null) return false;

            entity.Name = req.Name;
            entity.Description = req.Description;
            entity.CategoryId = req.CategoryId;
            entity.StandardDuration = req.StandardDuration;
            entity.Price = req.Price;
            entity.ImageUrl = req.ImageUrl;
            entity.IsOutdoor = req.IsOutdoor;
            entity.IsActive = req.IsActive;

            _repository.Update(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null) return false;

            entity.IsActive = isActive;
            _repository.Update(entity);
            return true;
        }
    }
}
