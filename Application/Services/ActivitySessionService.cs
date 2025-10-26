using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Service;
using Application.Interfaces.Repository;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ActivitySessionService : IActivitySessionService
    {
        private readonly IActivitySessionRepository _sessions;
        private readonly IMapper _mapper;

        public ActivitySessionService(IActivitySessionRepository sessions, IMapper mapper)
        {   _sessions = sessions;
            _mapper = mapper;
        }

        public async Task<ActivitySessionResponse> CreateAsync(CreateActivitySessionRequest req, CancellationToken ct)
        {
            // De här grundreglerna sitter även i FluentValidation – men vi behåller snabba skydd här.
            if (req.EndUtc <= req.StartUtc)
                throw new InvalidOperationException("Sluttiden måste vara efter starttiden.");
            if (req.Capacity <= 0)
                throw new InvalidOperationException("Kapaciteten måste vara större än 0.");

            var entity = new ActivitySession
            {
                ActivityId = req.ActivityId,
                LocationId = req.LocationId,
                StartUtc = req.StartUtc,
                EndUtc = req.EndUtc,
                Capacity = req.Capacity,
                IsCanceled = false
            };

            await _sessions.AddAsync(entity, ct); // repo sparar själv

            return new ActivitySessionResponse(
                entity.Id,
                entity.ActivityId,
                
                entity.LocationId,
                entity.StartUtc,
                entity.EndUtc,
                entity.Capacity,
                entity.IsCanceled,
                0 // valfri antal bokningar vid skapandet
            );
        }

        public async Task<IEnumerable<ActivitySessionResponse>> GetAllAsync()
        {
            var activities = await _sessions.GetAllAsync();

            return _mapper.Map<IEnumerable<ActivitySessionResponse>>(activities);
        }

        public async Task<IEnumerable<FilterActivitySessionResponse>> FilterAvailableSessionsAsync(
        FilterSessionsRequest request,
        CancellationToken ct = default)
        {
            var sessions = await _sessions.FilterAvailableSessionsAsync(
                request.StartDate,
                request.EndDate,
                request.CategoryId,
                request.IsIndoor,
                request.LocationId,
                ct);

            return _mapper.Map<IEnumerable<FilterActivitySessionResponse>>(sessions);
        }


        public async Task<ActivitySessionResponse> GetByIdAsync(int id)
        {
            var activitySession = await _sessions.GetByIdAsync(id);
            return activitySession == null ? null : _mapper.Map<ActivitySessionResponse>(activitySession);
        }
    }
}
