using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using Application.DTOs.Responses;
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
    }
}
