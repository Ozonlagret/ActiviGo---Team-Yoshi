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
    }
}
