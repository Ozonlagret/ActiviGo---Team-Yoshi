using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ActivitySessionService
    {
        private readonly IActivitySessionRepository _repository;

        public ActivitySessionService(IActivitySessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetAvailableSpotsAsync(int sessionId)
        {
            var session = await _repository.GetSessionWithBookingsAsync(sessionId);

            if (session == null)
                throw new InvalidOperationException($"Session with ID {sessionId} not found.");

            return session.Capacity - session.Bookings.Count;
        }
    }
}
